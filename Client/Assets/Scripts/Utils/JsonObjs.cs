/*
���� �ۼ�
�ۼ��� : 23.04.05

�ֱ� ���� ���� : 23.04.05
�ֱ� ���� ���� : json �Ľ̿� Ŭ������
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class CharacterstatHandler
    {
        public List<Characterstat> characterstats = new List<Characterstat>();

        public Characterstat this[Define.Charcter idx]
        {
            get => characterstats[(int)idx];
        }
    }

    [Serializable]
    public class Characterstat
    {
        public int AttackRange;
        public int SkillRange;
        public int AttackCool;
        public int SkillCool;
    }

    [Serializable]
    public class MonsterstatHandler 
    {
        public List<Monsterstat> monsterstats = new List<Monsterstat>();

        
        public Monsterstat this[Define.MonsterName idx]
        {
            get => monsterstats[(int)idx];
        }
    }

    [Serializable]
    public class Monsterstat
    {
        public string Name;
        public int MaxHP;
        public int AttackDMG;
        public float MoveSpeed;
        public float AttackSpeed;
        public float _offsetCorrection;
        public float _monsterHpBarOffset;
    }

}
