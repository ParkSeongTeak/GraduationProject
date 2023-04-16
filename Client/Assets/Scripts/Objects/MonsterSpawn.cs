using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    /// <summary>
    /// 4/15 ���� �ڼ��� ������
    /// 4/16 �ڼ��� ������Ʈ 4/12�� ���� ����� ���� ���� ������ ��ġ�� ������ �ݿ� 
    /// 
    /// </summary>
    public class MonsterSpawn : MonoBehaviour
    {
        #region ũ�� �ٲ� �� ���� ������
        GameObject[] SpawnPoint;
        GameObject Monster;
        int SpawnPointNum = 12;
        float Xradius = 10;
        float yradius = 5;
        /// <summary> ���� ���� Coroutine </summary>
        IEnumerator _startCreateMonster;
        /// <summary> ���� ���� Monster ���� </summary>
        Define.MonsterName _nowMonster;
        /// <summary> �̹� Wave ���� ���� ���� </summary>
        int _count;                                  
        /// <summary> Monster HP �ٿ��� Canvas�� Transform </summary>
        Transform _monsterHPCanvas;
        /// <summary> Monster �� HP�� �׸� Canvas</summary>
        public Transform MonsterHPCanvas { get { return _monsterHPCanvas; } }
        /// <summary> 0 ~ _cycle - 2 Wave ���� �Ϲ� ���� _cycle - 1 Wave ������ ���� ���� </summary>
        const int _cycle = 10;
        #endregion

        #region ������ ������ 
        /// <summary>  tmp ���� �� Wave���� ���� ���� ���� </summary>
        int _wavenum { get { return GameManager.InGameData.Wave % _cycle != _cycle - 1 ? (4 * (GameManager.InGameData.Wave / _cycle) + 2 * (GameManager.InGameData.Wave % _cycle) + 8) : 1 ; } }
        /// <summary> Monster ������ ���� Monster���ö����� �ð� ��  </summary>
        float _monsterToMonster { get { return (26.0f / (16.0f + GameManager.InGameData.Wave)); } }
        /// <summary>   Wave������(==�̹� Wave�� ������ ���� ���� ����) ���� Wave ���� ������ �ð� �� </summary>
        float _waveToWave = 8.0f;
        #endregion

        /// <summary> ��ȯ�� ���� ���� </summary>
        List<MonsterController> _monsters = new List<MonsterController>();
        /// <summary> ��ȯ�� ���� ���� </summary>
        public List<MonsterController> Monsters { get { return _monsters; } }
        //InstantiateMonster�� ���������� �� ���� ������ ȿ�� (Enum�� Ȱ���Ͽ� Monster�� �̸��� �ܿ� �ʿ並 ����, ����Wave�� ++������ ���ϰ�)
        GameObject InstantiateMonster(Define.MonsterName monster,Transform SpawnPoint)
        {
            string monsterStr = Enum.GetName(typeof(Define.MonsterName), monster);
            return GameManager.Resource.Instantiate($"Monster/{monsterStr}", SpawnPoint);
        }


        void init()
        {
            SpawnPoint = new GameObject[SpawnPointNum];
            _nowMonster = Define.MonsterName._0BlackBoar;
            _monsterHPCanvas = GameManager.UI.ShowSceneUI<UI_MonsterHP>().transform;
            _startCreateMonster = StartCreateMonster();
            _count = 0;
            for (int i = 0; i < SpawnPointNum; i++)
            {
                SpawnPoint[i] = new GameObject { name = $"SpawnPoint{i}" };
                SpawnPoint[i].transform.parent = transform;
                SpawnPoint[i].transform.position = new Vector3(Mathf.Cos(((2 * Mathf.PI) / SpawnPointNum) * i) * Xradius,
                    Mathf.Sin(((2 * Mathf.PI) / SpawnPointNum) * i) * yradius, 0);
            }

        }
        
        // Start is called before the first frame update
        void Start()
        {
            init();
            StartCoroutine(_startCreateMonster);
        }

        public bool WaveEnd()
        {
            return _nowMonster == Define.MonsterName.MaxCount;
        }

        IEnumerator StartCreateMonster()
        {
            while (GameManager.InGameData.CurrState == Define.State.Play)
            {   
                //���� ����
                if (_count < _wavenum)
                {
                    MonsterController mon = InstantiateMonster((Define.MonsterName)(GameManager.InGameData.Wave), SpawnPoint[UnityEngine.Random.Range(0, SpawnPointNum)].transform).GetComponent<MonsterController>();
                    _monsters.Add(mon);
                    yield return new WaitForSecondsRealtime(_monsterToMonster * 0.1f);
                    _count += 1;

                }
                //���� Wave
                else
                {
                    _count = 0;
                    GameManager.InGameData.Wave += 1;

                    if (_nowMonster == Define.MonsterName.MaxCount)
                    {
                        break;
                    }
                    yield return new WaitForSecondsRealtime(_waveToWave);

                }
            }
        }



    }
}