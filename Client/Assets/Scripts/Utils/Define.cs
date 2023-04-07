using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Define
    {

        public enum Charcter
        {
            Warrior,
            Rifleman,
            Wizard,
            Priest,
            MaxCount
        }

        public enum Sound
        {
            BGM,
            SFX,
            MaxCount
        }
        public enum BGM
        {



            MaxCount
        }
        public enum SFX
        {
            MaxCount
        }
        public enum State
        {
            Play,
            End,
            Pause,
            MaxCount
        }


        public enum Item
        {
            ������_������_����,
            ĥ��ĥ��_��ȯ����_Class,
            ����_�Ϸ�����_���ư�����_DB,
            �︮��_����_�ڸ���,
            MaxCount
        }
        public enum Tag
        {
            Monster,
            Tower,
            MaxCount
        }

        /// <summary>
        /// UI Event ���� ����
        /// </summary>
        public enum UIEvent
        {
            Click,
            Drag,
            DragEnd,
        }

        public enum Scenes
        { 
            Title,
            Game,
        }

        public enum MonsterName
        {
            Bat,
            BlackBoar,
            BlackBear,
            BlackWolf,
            CaveRat,
            Cerberus,
            Crawler,
            CrystalLizard,
            DreadEye,
            MinerBoar,
            Nightmare,
            Porcupine,
            PurpleScarab,
            Scarab,
            ShardLizard,
            SlugQueen,
            Warg,
            MaxCount
        }

        public enum MonsterState
        {
            Idle,
            Attack,
            Run,
            Walk,
            Death,
            MaxCount
        }

        
    }
}