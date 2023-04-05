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
            Wizard,
            Priest,
            Rifleman,
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

    }
}