﻿/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.16
최근 수정 내용 : 공격 애니메이션 동기화, 사제 버프, 아이템 동기화 구현
 ******/

using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    public class Room
    {
        public string RoomName;
        IngameData _ingameData = new IngameData();
        Random random = new Random();

        /// <summary> 현재 방에 존재하는 클라이언트들 </summary>
        List<ClientSession> _sessions = new List<ClientSession>();
        /// <summary> 작업 관리 queue </summary>
        JobQueue _jobQueue = new JobQueue();
        /// <summary> broadcasting 대기 중인 데이터들 </summary>
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    

        /// <summary> 새로운 작업 수행 예약 </summary>
        public void Push(Action job) => _jobQueue.Push(job);

        /// <summary> 방 인원 수 </summary>
        public int Count => _sessions.Count;


        //job queue에서 수행하기 때문에 싱글 쓰레드 가정
        #region Jobs
        /// <summary> 대기 중인 broadcasting 모두 수행 </summary>
        public void Flush()
        {
            if (_pendingList.Count > 0)
                foreach (ClientSession session in _sessions)
                    session.Send(_pendingList);

            _pendingList.Clear();
        }

        /// <summary> 브로드캐스팅 예약 </summary>
        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        #region ReadyToPlay
        /// <summary> 새로운 클라이언트 입장 </summary>
        public void Enter(ClientSession session)
        {
            //풀방 -> 입장 불가
            if(_sessions.Count >= RoomManager.MAX_PLAYER_COUNT)
            {
                STC_RejectEnter_Full fullPacket = new STC_RejectEnter_Full();
                session.Send(fullPacket.Write());
                return;
            }

            _sessions.Add(session);
            session.Room = this;

            STC_ExistPlayers existPacket = new STC_ExistPlayers();
            foreach (var s in _sessions)
                existPacket.Players.Add(s.SessionId);

            session.Send(existPacket.Write());

            STC_PlayerEnter enterPacket = new STC_PlayerEnter();
            enterPacket.playerId = session.SessionId;

            Broadcast(enterPacket.Write());
        }

        /// <summary> 클라이언트 퇴장 </summary>
        public void Leave(ClientSession session)
        {
            //모든 유저가 나간 방 제거
            if (_sessions.Count <= 1)
            {
                RoomManager.Instance.Push(() => RoomManager.Instance.Remove(this));
                _sessions.Clear();
                RoomName = string.Empty;
                _ingameData = null;
                return;
            }

            //방장이 나갈 경우, 방장 변경
            if(_sessions.IndexOf(session) <= 0)
            {
                STC_SetSuper superPacket = new STC_SetSuper();
                _sessions[1].Send(superPacket.Write());
            }

            _sessions.Remove(session);

            STC_PlayerLeave leavePacket = new STC_PlayerLeave();
            leavePacket.playerId = session.SessionId;

            Broadcast(leavePacket.Write());
        }

        /// <summary> 로비 -> 캐릭터 선택 전환 </summary>
        public void Ready(ClientSession session)
        {
            _ingameData.Init(_sessions);

            STC_ReadyGame readyPacket = new STC_ReadyGame();

            Broadcast(readyPacket.Write());
        }

        /// <summary> 클라이언트의 직업 선택 </summary>
        public void SelectClass(ClientSession session, Define.PlayerClass playerClass)
        {
            bool allSelected = _ingameData.SelectClass(session.SessionId, playerClass);

            //선택 정보 다른 클라이언트에 공유
            STC_SelectClass selectPacket = new STC_SelectClass();
            selectPacket.PlayerId = session.SessionId;
            selectPacket.PlayerClass = (ushort)playerClass;
            Broadcast(selectPacket.Write());

            //모두 선택 시, 게임 시작
            if(allSelected)
            {
                STC_StartGame startPacket = new STC_StartGame();
                Broadcast(startPacket.Write());

                JobTimer.Instance.Push(Start, 100);
            }
        }
        #endregion ReadyToPlay

        #region Ingame
        /// <summary> 플레이어 이동 동기화 </summary>
        /// <param name="session"> 이동 패킷 보낸 세션 </param>
        public void Move(ClientSession session, CTS_PlayerMove movePacket)
        {
            STC_PlayerMove packet = new STC_PlayerMove();
            packet.playerId = session.SessionId;
            packet.posX = movePacket.posX;
            packet.posY = movePacket.posY;

            Broadcast(packet.Write());
        }

        /// <summary> 플레이어 공격 애니메이션 동기화 </summary>
        public void Attack(ClientSession session, CTS_PlayerAttack attackPacket)
        {
            STC_PlayerAttack packet =  new STC_PlayerAttack();
            packet.playerId = session.SessionId;
            packet.skillType= attackPacket.skillType;
            packet.direction = attackPacket.direction;

            Broadcast(packet.Write());
        }

        /// <summary> 사제 버프 </summary>
        public void Buff(CTS_PriestBuff buffPacket)
        {
            STC_PriestBuff packet = new STC_PriestBuff();
            packet.buffRate = buffPacket.buffRate;

            ClientSession? session = _sessions.Find(x => x.SessionId == buffPacket.playerId);

            if (session != null)
                session.Send(packet.Write());
        }

        /// <summary> 아이템 정보 동기화 </summary>
        public void ItemUpdate(ClientSession session, CTS_ItemUpdate itemPacket)
        {
            STC_ItemUpdate updatePacket = new STC_ItemUpdate();
            updatePacket.playerId = session.SessionId;
            updatePacket.position = itemPacket.position;
            updatePacket.itemIdx = itemPacket.itemIdx;

            Broadcast(updatePacket.Write());
        }
        /// <summary> 몬스터 HP 동기화 </summary>
        public void MonsterHPUpdate(CTS_MonsterHPUpdate monsterHPPacket)
        {
            STC_MonsterHPUpdate updatePacket = new STC_MonsterHPUpdate();
            
            updatePacket.ID = monsterHPPacket.ID;
            updatePacket.updateHP = monsterHPPacket.updateHP;

            Broadcast(updatePacket.Write());
        }
        /// <summary> Monster 생성 </summary>
        public void CreateMonster()
        {
            if (_ingameData == null)
                return;


            STC_MosterCreate packet = new STC_MosterCreate();
            packet.createIDX = (ushort)random.Next(0,12);
            packet.ID = _ingameData.MonsterControlInfo.NextMosterID;
            packet.typeNum = _ingameData.MonsterControlInfo.MonsterTypeNum;
            _ingameData.MontersDic[_ingameData.MonsterControlInfo.NextMosterID] = new MonsterInfo(packet.ID, _ingameData.monsterStatHandler.monsterstats[packet.typeNum].MaxHP);
            _ingameData.MonsterControlInfo.MonsterTypePlus();
            Console.WriteLine($"위치: {packet.createIDX} \t 몬스터 ID: {packet.ID} \t 몬스터 type: {packet.typeNum}");

            Broadcast(packet.Write());
            if (_ingameData.MonsterControlInfo.MonsterTypeNum >= 59)
            {
                Console.WriteLine("End");
                _ingameData.State = IngameData.state.EndWave;
            }
        }

        public async void Start()
        {
            _ingameData.State = IngameData.state.Play;
            await Task.Delay(TimeSpan.FromSeconds(1.0f));

            while (_ingameData.State == IngameData.state.Play)
            {

                //몬스터 생성
                CreateMonster();
                Console.WriteLine("_ingameData.MonsterControlInfo.MonsterToMonster: " + _ingameData.MonsterControlInfo.MonsterToMonster);
                await Task.Delay(TimeSpan.FromSeconds(_ingameData.MonsterControlInfo.MonsterToMonster));

                if (_ingameData == null)
                    break;

                //다음 Wave
                if (_ingameData.MonsterControlInfo.NextWave)
                {
                    Console.WriteLine("------------------");
                    Console.WriteLine("_ingameData.MonsterControlInfo.WaveToWave; " + _ingameData.MonsterControlInfo.WaveToWave);
                    await Task.Delay(TimeSpan.FromSeconds(_ingameData.MonsterControlInfo.WaveToWave));
                    _ingameData.MonsterControlInfo.NextWave = false;
                    Console.WriteLine("------------------");
                }
            }
        }
        #endregion Ingame
        #endregion Jobs
    }
}