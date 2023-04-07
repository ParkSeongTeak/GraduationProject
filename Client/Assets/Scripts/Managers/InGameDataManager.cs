using System;
using System.Collections.Generic;
using UnityEngine;
using static Client.Define;

namespace Client
{
    public class InGameDataManager
    {
        GameObject _gameOver;
        public GameObject GameOver { get { return _gameOver; } }


        #region Score & Money       
        int _money = 0;
        public int Money { get { return _money; } set { 
                _money = value; 
                UI_GameScene.TextsAction.Invoke();
                UI_GetItem.ShopAction?.Invoke();
            } 
        }

        /* public readonly*/
        int _itemCost = 10;
        /// <summary>
        /// �̰� ���� ������ ���� ���� �ֳ�?
        /// </summary>
        public int ItemCost { get { return _itemCost; } set { _itemCost = value; } }

        int _moneyRewards = 5;
        int _scoreRewards = 1;
        /// <summary>
        /// Wave ���� �÷�����ϳ�?
        /// </summary>
        public int MoneyRewards { get { return _moneyRewards; } }
        public int ScoreRewards { get { return _scoreRewards; } }

        int _score = 0;
        public int Score { get { return _score; } 
            set { _score = value; 
                UI_GameScene.TextsAction.Invoke();
            } 
        }

        #endregion

        #region Player
        /// <summary>
        /// �÷��̾� ��Ÿ�� ��Ʈ�ѷ�
        /// </summary>
        public CooldownController Cooldown { get; } = new CooldownController();
        /// <summary>
        /// ��� ������ ���� ���� ����
        /// </summary>
        public CharacterstatHandler CharacterStat { get; private set; }
        /// <summary>
        /// ���� ���ӿ� ������ �÷��̾� ĳ���� ������Ʈ��
        /// </summary>
        List<PlayerController> _playerControllers = new List<PlayerController>();
        /// <summary>
        /// Ŭ���̾�Ʈ�� ĳ����
        /// <para></para>
        /// </summary>
        public PlayerController MyPlayer { 
            get
            {
                if (_playerControllers.Count > 0)
                    return _playerControllers[0];

                return null;
            }
        }
        /// <summary>
        /// ���� ���� ���� ���� ����� �÷��̾�
        /// </summary>
        public PlayerController NearPlayer => null;
        #endregion Player

        #region Item

        public readonly int MAXITEMNUM = 8;
        Item[] _itemDB;
        List<Item> _myInventory;
        public List<Item> MyInventory { get { return _myInventory; } }
        public Item[] ItemDB { get { return _itemDB;} }



        public void MyInventoryRandomADD() {
            if (_myInventory.Count < MAXITEMNUM)
            {
                _myInventory.Add(_itemDB[UnityEngine.Random.Range(0, _itemDB.Length)]);
            }
            else
            {

            }
        }

        public void MyInventoryDelete(Item item)
        {

        }
        #endregion


        #region Monster
        public MonsterstatHandler MonsterStates { get; private set; }

        MonsterSpawn _monsterSpawn;
        TowerController _tower;
        GameObject _monsterHpBar;



        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }
        public TowerController Tower { get { return _tower; } }
        public GameObject MonsterHpBar { get { return _monsterHpBar; } }
        #endregion

        #region state machine

        bool[] _state = new bool[(int)Define.State.MaxCount];



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

        /// <summary>
        /// 
        /// </summary>
        public void init()
        {
            _itemDB = new Item[(int)Define.Item.MaxCount];
            string[] _itemDBstr = Enum.GetNames(typeof(Define.Item));
            
            for (int i = 0; i < (int)Define.Item.MaxCount; i++)
            {
                _itemDB[i] = new Item();
                _itemDB[i].Name = _itemDBstr[i];
            }

            _myInventory = new List<Item>();

            CharacterStat = Util.ParseJson<CharacterstatHandler>();
            MonsterStates = Util.ParseJson<MonsterstatHandler>();

            _monsterHpBar = Resources.Load<GameObject>("Prefabs/UI/MonsterHP");
        }

        /// <summary>
        /// ���ο� ���� ���� - ���� ���� ��ġ�� �߾� Ÿ�� ����
        /// </summary>
        public void GameStart()
        {
            GenerateMonsterSpawnPoint();
            GenerateTower();
            GeneratePlayer();
        }

        #region GameStart_Generate
        /// <summary> ���� ���� ����Ʈ ���� </summary>
        void GenerateMonsterSpawnPoint()
        {
            GameObject monsterSpawn = GameObject.Find("MonsterSpawn");
            if (monsterSpawn == null)
                monsterSpawn = GameManager.Resource.Instantiate("Monster/MonsterSpawn/MonsterSpawn");
            _monsterSpawn = monsterSpawn.GetComponent<MonsterSpawn>();
        }
        /// <summary> �߾� Ÿ�� ���� </summary>
        void GenerateTower()
        {
            GameObject tower = GameObject.Find("Tower");
            if (tower == null)
                tower = GameManager.Resource.Instantiate("Tower/Tower");
            _tower = tower.GetComponent<TowerController>();
        }
        /// <summary> �÷��̾� ���� 
        /// <para>����� �ϳ��� ����������, ���߿� ���� �ο�����ŭ ���� </para>
        /// </summary>
        void GeneratePlayer()
        {
            int classIdx = PlayerPrefs.GetInt("Class", 0);
            GameObject playerGO;
            PlayerController playerController = null;
            switch (classIdx)
            {
                case 0:
                    playerGO = GameManager.Resource.Instantiate("Player/Warrior");
                    playerController = Util.GetOrAddComponent<Warrior>(playerGO);
                    break;
                case 1:
                    playerGO = GameManager.Resource.Instantiate("Player/Rifleman");
                    playerController = Util.GetOrAddComponent<Rifleman>(playerGO);
                    break;
                case 2:
                    playerGO = GameManager.Resource.Instantiate("Player/Wizard");
                    playerController = Util.GetOrAddComponent<Wizard>(playerGO);
                    break;
                default:
                    playerGO = GameManager.Resource.Instantiate("Player/Priest");
                    playerController = Util.GetOrAddComponent<Priest>(playerGO);
                    break;
            }

            playerGO.transform.position = Vector3.down;
            _playerControllers.Add(playerController);
        }
        #endregion GameStart_Generate

        public void Clear()
        {
            _money = _score = 0;
            Cooldown.Clear();
            _myInventory.Clear();
        }
    }

}
