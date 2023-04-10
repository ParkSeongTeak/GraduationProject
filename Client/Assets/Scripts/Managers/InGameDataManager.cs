using System;
using System.Collections.Generic;
using UnityEngine;
using static Client.Define;

namespace Client
{
    public class InGameDataManager
    {
        #region Money   
        /// <summary> ���� ���� �� </summary>
        int _money = 0;
        /// <summary> set ��, ������ ���� �� ���� UI ������Ʈ </summary>
        public int Money
        {
            get { return _money; }
            set
            {
                _money = value;
                UI_GameScene.TextChangeAction?.Invoke();
                UI_GetItem.OnMoneyChangedAction?.Invoke();
            }
        }

        /// <summary>
        /// Wave ���� �÷�����ϳ�?
        /// </summary>
        int _moneyRewards = 5;
        public int MoneyRewards { get { return _moneyRewards; } }
        #endregion Money

        #region Score
        int _scoreRewards = 1;
        public int ScoreRewards { get { return _scoreRewards; } }

        int _score = 0;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                UI_GameScene.TextChangeAction?.Invoke();
            }
        }
        #endregion Score

        #region Item
        /// <summary> ������ �ִ� ���� ���� �� </summary>
        public readonly int MAXITEMCOUNT = 8;
        /// <summary> ������ ���� ���� ���� ��ȯ </summary>
        public bool CanBuyItem { get => _money >= _itemCost; }
        /// <summary>
        /// �̰� ���� ������ ���� ���� �ֳ�?
        /// </summary>
        int _itemCost = 10;
        public int ItemCost { get { return _itemCost; } set { _itemCost = value; } }

        /// <summary> ��� ������ ���� </summary>
        Item[] _itemDB;
        /// <summary> ���� ���� ���� ������ ���� </summary>
        List<Item> _myInventory = new List<Item>();
        /// <summary> ���� ���� ���� ������ ���� </summary>
        public List<Item> MyInventory { get { return _myInventory; } }

        /// <summary> ���ο� ������ ���� </summary>
        public void AddRandomItem() 
        {
            if (_myInventory.Count < MAXITEMCOUNT)
                _myInventory.Add(_itemDB[UnityEngine.Random.Range(0, _itemDB.Length)]);
            else
            {
                //TODO : ���� ������ ����
            }
        }
        /// <summary> ���� ���� ������ ������ </summary>
        public void MyInventoryDelete(int idx)
        {

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
        public CharacterstatHandler CharacterStats { get; private set; }
        /// <summary>
        /// ���� ���ӿ� ������ �÷��̾� ĳ���� ������Ʈ��
        /// </summary>
        List<PlayerController> _playerControllers = new List<PlayerController>();
        /// <summary>
        /// Ŭ���̾�Ʈ�� ĳ����
        /// <para></para>
        /// </summary>
        public PlayerController MyPlayer
        {
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

        #region Monster
        /// <summary> ��� ���� ���� ���� </summary>
        public MonsterstatHandler MonsterStats { get; private set; }

        /// <summary> ���� ��ȯ ���� Ŭ���� </summary>
        MonsterSpawn _monsterSpawn;
        /// <summary> ���� ��ȯ ���� Ŭ���� </summary>
        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }

        /// <summary> ���� ü�¹� ������ </summary>
        GameObject _hpBarPrefab;
        /// <summary> ���� ü�¹� ������ </summary>
        public GameObject HPBarPrefab { get { return _hpBarPrefab; } }
        #endregion

        /// <summary> �߾� Ÿ�� </summary>
        TowerController _tower;
        /// <summary> �߾� Ÿ�� </summary>
        public TowerController Tower { get { return _tower; } }

        #region state machine
        /// <summary> ���� ���� ���� </summary>
        bool[] _state = new bool[(int)Define.State.MaxCount];
        /// <summary>
        /// ���� �̷������� ����δ� ������ Play������ ���� ��ġ �غ��� Play�غ��� �ͱ� ���� 
        /// ��ġ �ϵ� ���� �ϳ��� �� ��������
        /// </summary>
        public void StateChange(State state)
        {
            for (State iterator = 0; iterator < State.MaxCount; iterator++)
                _state[(int)iterator] = state == iterator;
        }
        /// <summary> ���� ���� ��ȯ </summary>
        public State CurrState()
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

        /// <summary> ������ db �ʱ�ȭ, ���� ����, ������ �ҷ����� </summary>
        public void init()
        {
            _itemDB = new Item[(int)Define.Item.MaxCount];
            string[] _itemDBstr = Enum.GetNames(typeof(Define.Item));
            
            for (int i = 0; i < (int)Define.Item.MaxCount; i++)
            {
                _itemDB[i] = new Item();
                _itemDB[i].Name = _itemDBstr[i];
            }

            CharacterStats = Util.ParseJson<CharacterstatHandler>();
            MonsterStats = Util.ParseJson<MonsterstatHandler>();

            _hpBarPrefab = GameManager.Resource.Load<GameObject>("Prefabs/UI/MonsterHP");
        }

        /// <summary> ���ο� ���� ���� - ���� ���� ��ġ�� �߾� Ÿ�� ���� </summary>
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
        /// <summary> �÷��̾� ���� <br/>
        /// ����� �ϳ��� ����������, ���߿� ���� �ο�����ŭ ���� </summary>
        void GeneratePlayer()
        {
            _playerControllers.Clear();

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

        /// <summary> ���� �÷��� ���� �ʱ�ȭ </summary>
        public void Clear()
        {
            _money = _score = 0;
            Cooldown.Clear();
            _myInventory.Clear();
        }
    }

}
