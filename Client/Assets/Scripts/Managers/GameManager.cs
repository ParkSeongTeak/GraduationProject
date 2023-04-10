/*
���� �ۼ�
�ۼ��� : 23.03.29

�ֱ� ������ : �̿쿭
�ֱ� ���� ���� : 23.04.10
�ֱ� ���� ���� : �ּ� ����
*/
using UnityEngine;

namespace Client
{
    public class GameManager : MonoBehaviour
    {
        static GameManager _instance;
        static GameManager Instance { get { Init(); return _instance; } }
        #region Managers
        //InputManager _inputManager = new InputManager();
        NetworkManager _networkManager = new NetworkManager();
        PoolManager _poolManager = new PoolManager();
        ResourceManager _resourceManager = new ResourceManager();
        SoundManager _soundManager = new SoundManager();
        InGameDataManager _inGameDataManager = new InGameDataManager();
        UIManager _uiManager = new UIManager();
        //public static InputManager Input { get { return Instance._inputManager; } }
        public static NetworkManager Network { get { return Instance._networkManager; } }
        public static PoolManager Pool { get { return Instance._poolManager; } }
        public static ResourceManager Resource { get { return Instance._resourceManager; } }
        public static SoundManager Sound { get { return Instance._soundManager; } }
        public static InGameDataManager InGameData { get { return Instance._inGameDataManager; } }
        public static UIManager UI { get { return Instance._uiManager; } }
        #endregion

        /// <summary> instance ����, ���� �Ŵ����� �ʱ�ȭ </summary>
        static void Init()
        {
            if (_instance == null)
            {
                GameObject gm = GameObject.Find("GameManager");
                if (gm == null)
                {
                    gm = new GameObject { name = "GameManager" };
                    gm.AddComponent<GameManager>();
                }
                _instance = gm.GetComponent<GameManager>();
                DontDestroyOnLoad(gm);

                //_instance._inputManager.init();
                _instance._networkManager.Init();
                _instance._poolManager.Init();
                _instance._soundManager.Init();
                _instance._inGameDataManager.Init();
            }

        }
        /// <summary> ���� ����, ���� ����, �ΰ��� ���� �ʱ�ȭ </summary>
        public static void GameStart()
        {
            Time.timeScale = 1;
            InGameData.StateChange(Define.State.Play);
            _instance._inGameDataManager.GameStart();
        }
        /// <summary> �¸� �Ǵ� �й� �� ȣ��, �ð� ����, ���� ����, UI ���� </summary>
        public static void GameOver(Define.State endState)
        {
            if (endState == Define.State.Win || endState == Define.State.Defeat)
            {
                Time.timeScale = 0;
                InGameData.StateChange(endState);

                _instance._uiManager.ShowPopUpUI<UI_GameOver>();
            }
        }

        /// <summary> ��� ���� �ʱ�ȭ </summary>
        public static void Clear()
        {
            //_instance._inputManager.Clear();
            _instance._networkManager.Clear();
            _instance._poolManager.Clear();
            _instance._resourceManager.Clear();
            _instance._soundManager.Clear();
        }

    }
}
