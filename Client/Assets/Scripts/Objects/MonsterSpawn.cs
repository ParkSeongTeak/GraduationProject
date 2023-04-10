using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class MonsterSpawn : MonoBehaviour
    {

        /// <summary>
        /// ������ ��Ծ �صδ� �޸�
        /// FantasyMonsters ���� ���� ���� �ٲٸ� Ÿ ���α׷��ӿ� ���� �ȵ��ư� 
        /// </summary>
        /// 

        GameObject[] SpawnPoint;
        GameObject Monster;
        int SpawnPointNum = 12;
        float Xradius = 10;
        float yradius = 5;

                                                    
        IEnumerator _startCreateMonster;            // ���� ���� Coroutine
        Define.MonsterName _nowMonster;             // ���� ���� Monster ����
        float MonsterToMonster = 0.25f;              // Monster ������ ���� Monster���ö����� �ð� �� 
        float WaveToWave = 1.0f;                    // Wave������(==�̹� Wave�� ������ ���� ���� ����) ���� Wave ���� ������ �ð� ��
        int Wavenum = 3;                            // tmp ���� �� Wave���� ���� ���� ���� (���� Wave���� ���������� ������ ��)
        int Count;                                  // �̹� Wave ���� ���� ����
        Transform _monsterHPCanvas;                 // Monster HP �ٿ��� Canvas�� Transform
        public Transform MonsterHPCanvas { get { return _monsterHPCanvas; } }

        /// <summary>
        /// 
        /// </summary>
        List<MonsterController> _monsters = new List<MonsterController>();
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
            _nowMonster = Define.MonsterName.BlackBoar;
            _monsterHPCanvas = GameManager.UI.ShowSceneUI<UI_MonsterHP>().transform;
            _startCreateMonster = StartCreateMonster();
            Count = 0;
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
            while (GameManager.InGameData.CurrState() == Define.State.Play)
            {
                if (Count < Wavenum)
                {
                    MonsterController mon = InstantiateMonster(_nowMonster, SpawnPoint[UnityEngine.Random.Range(0, SpawnPointNum)].transform).GetComponent<MonsterController>();
                    _monsters.Add(mon);
                    yield return new WaitForSecondsRealtime(MonsterToMonster);
                    Count += 1;
                }
                else
                {
                    Count = 0;
                    _nowMonster += 1;
                    if(_nowMonster == Define.MonsterName.MaxCount)
                    {
                        break;
                    }
                    yield return new WaitForSecondsRealtime(WaveToWave);

                }
            }
        }



    }
}