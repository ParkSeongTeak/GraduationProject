using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        /// <summary> �⺻ ���� ������ ��� </summary>
        protected float _attackDMGRatio;
        /// <summary> ��ų ������ ��� </summary>
        protected float _skillDMGRatio;

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
        public Define.Charcter MyClass { get; protected set; }

        /// <summary> ���� ����, ���� ���� ���� </summary>
        public virtual void IsAttack()
        {
            //��Ÿ�� ���� �ƴ� ��
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                //��Ÿ� ���� ���Ͱ� ������ ��
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.AttackRange)
                {
                    mon.BeAttacked(Mathf.RoundToInt(_attackDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(stat.AttackCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("attack cool");

        }
        /// <summary> ��ų ����, ���� ���� ���� </summary>
        public virtual void IsSkill()
        {
            //��Ÿ�� ���� �ƴ� ��
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                //��Ÿ� ���� ���Ͱ� ������ ��
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    mon.BeAttacked(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("skill cool");
        }
        /// <summary> �÷��̾�� ���� �� ����, �� �Լ� </summary>
        protected sealed override void Dead() { }

        #region Move
        /// <summary>
        /// ���� �÷��̾� ����
        /// </summary>
        PlayerState _state = PlayerState.Idle;
        /// <summary>
        /// �̵� ���� ����
        /// </summary>
        Vector2 _moveDirection = Vector2.zero;
        private void FixedUpdate()
        {
            if (_state == PlayerState.Move)
                IsMove();
        }

        public void IsMove() => transform.Translate(_moveDirection * Time.deltaTime * MoveSpeed);
        
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
        #endregion Move

        #region TargetSelect
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
        /// <summary>
        /// �����ڿ��� �ִ� ��Ÿ����� �̾����� ���� ���� ������Ʈ ����
        /// </summary>
        /// <param name="range">������ ��Ÿ�</param>
        /// <param name="enemyPos">Ÿ�� ��ġ</param>
        protected RangedArea GenerateRangedArea(int range, Vector3 enemyPos)
        {
            GameObject rangedArea = GameManager.Resource.Instantiate("Player/RangedArea");
            rangedArea.transform.localScale = new Vector3(1, range, 1);

            float angle = Mathf.Atan2(enemyPos.y - transform.position.y, enemyPos.x - transform.position.x) * Mathf.Rad2Deg;
            rangedArea.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


            Vector3 dir = (enemyPos - transform.position).normalized;
            rangedArea.transform.position = gameObject.transform.position + dir * range / 2f;

            return Util.GetOrAddComponent<RangedArea>(rangedArea);
        }

        /// <summary>
        /// Ÿ�� ������ ���� ���� ������Ʈ ����
        /// </summary>
        /// <param name="range"> ���� ������Ʈ�� �ݰ� </param>
        /// <param name="enemyPos"> Ÿ�� ��ġ </param>
        protected RangedArea GenerateTargetArea(int range, Vector3 enemyPos)
        {
            GameObject targetArea = GameManager.Resource.Instantiate("Player/TargetArea");
            targetArea.transform.localScale = new Vector3(range, range, 1);

            targetArea.transform.position = enemyPos;
            return Util.GetOrAddComponent<RangedArea>(targetArea);
        }
        #endregion TargetSelect
    }
}
