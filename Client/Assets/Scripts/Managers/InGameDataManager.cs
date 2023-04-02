using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Client.Define;

namespace Client
{
    public class InGameDataManager
    {
        MonsterSpawn _monsterSpawn;
        TowerController _tower;
        GameObject _monsterHpBar;
        GameObject _gameOver;


        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }
        public TowerController Tower { get { return _tower; } }
        public GameObject MonsterHpBar { get { return _monsterHpBar; } }
        public GameObject GameOver { get { return _gameOver; } }


        #region state machine

        bool[] _state;



        /// <summary>
        /// ���� �̷������� ����δ� ������ Play������ ���� ��ġ �غ��� Play�غ��� �ͱ� ���� 
        /// ��ġ �ϵ� ���� �ϳ��� �� ��������
        /// </summary>


        public void StateChange(State nowState)
        {
            for (State stat = 0; stat < State.MaxCount; stat++)
            {
                if (nowState == stat)
                {
                    _state[(int)stat] = true;
                }
                else
                {
                    _state[(int)stat] = false;
                }
            }

        }
        public State Stat()
        {
            for (State stat = 0; stat < State.MaxCount; stat++)
            {
                if (_state[(int)stat])
                {
                    return stat;
                }
            }
            return State.End;
        }
        #endregion

        public void init()
        {
            if (_state == null)
            {
                _state = new bool[(int)State.MaxCount];
            }
            if (_monsterSpawn == null)
            {
                GameObject monsterSpawn = GameObject.Find("MonsterSpawn");
                if (monsterSpawn == null)
                {
                    monsterSpawn = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Monster/MonsterSpawn"));
                    monsterSpawn.name = "MonsterSpawn";
                }
                _monsterSpawn = monsterSpawn.GetComponent<MonsterSpawn>();
            }

            if (_tower == null)
            {
                GameObject tower = GameObject.Find("Tower");
                if (tower == null)
                {
                    tower = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Tower/Tower"));
                    tower.name = "Tower";
                }
                _tower = tower.GetComponent<TowerController>();

            }

            _monsterHpBar = Resources.Load<GameObject>("Prefabs/UI/MonsterHP");
            _gameOver = Resources.Load<GameObject>("Prefabs/UI/GameOver");


        }

    }

}
