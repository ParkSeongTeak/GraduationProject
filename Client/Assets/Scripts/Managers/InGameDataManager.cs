using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Client.Define;

namespace Client
{
    public class InGameDataManager
    {
        #region state machine

        bool[] _state;
        MonsterSpawn _monsterSpawn;
        TowerController _tower;

        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }
        public TowerController Tower { get { return _tower; } }

        /// <summary>
        /// ���� �̷������� ����δ� ������ Play������ ���� ��ġ �غ��� Play�غ��� �ͱ� ���� 
        /// ��ġ �ϵ� ���� �ϳ��� �� ��������
        /// </summary>
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


        }

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


    }

}
