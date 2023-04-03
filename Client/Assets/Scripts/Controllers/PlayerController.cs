using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        enum PlayerState
        { 
            Idle,
            Move,
            Attack,
            Skill,
        }

        /// <summary>
        /// �� ����
        /// </summary>
        protected Define.Charcter _myClass;
        /// <summary>
        /// ���� ������ �ִ� ������ ����Ʈ
        /// </summary>
        List<Item> _items = new List<Item>();

        public List<Item> MyItems { get { return _items; } }
        public Define.Charcter MyClass { get { return _myClass; } }

        /// <summary>
        /// ���� �÷��̾� ����
        /// </summary>
        PlayerState _state = PlayerState.Idle;
        /// <summary>
        /// �̵� ���� ����
        /// </summary>
        Vector2 _moveDirection = Vector2.zero;

        // ������ �� �� ������ Status.BeAttacked(float DMG)���� ����
        public abstract void IsAttack();
        public abstract void IsSkill();
        protected override void Dead() { }
        public void IsMove()
        {
            transform.Translate(_moveDirection * Time.deltaTime * MoveSpeed);
        }

        #region Joystick
        /// <summary>
        /// ���̽�ƽ�� ���� ���� ����
        /// </summary>
        /// <param name="dir"></param>
        public void SetDirection(Vector2 dir)
        {
            _state = PlayerState.Move;
            _moveDirection = dir;
        }
        /// <summary>
        /// ���̽�ƽ ���� ����
        /// </summary>
        public void StopMove() => _state = PlayerState.Idle;
        #endregion Joystick

        private void FixedUpdate()
        {
            if (_state == PlayerState.Move)
                IsMove();
        }

        /// <summary>
        /// ���� ����� ���� ��ȯ
        /// </summary>
        protected MonsterController NearMoster()
        {
            List<MonsterController> monsters = GameManager.InGameData.MonsterSpawn.Monsters;

            if (monsters.Count <= 0)
                return null;

            MonsterController nearMon = monsters[0];
            float pivotDis = Vector3.Distance(transform.position, nearMon.transform.position);
            for(int i = 1; i < monsters.Count;i++)
            {
                float currDis = Vector3.Distance(transform.position, monsters[i].transform.position);
                if (currDis < pivotDis)
                {
                    pivotDis = currDis;
                    nearMon = monsters[i];
                }
            }

            return nearMon;
        }
    }
}
