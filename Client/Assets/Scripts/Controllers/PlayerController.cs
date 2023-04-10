/*
�ۼ��� : �̿쿭
�ۼ��� : 23.03.29
�ֱ� ���� ���� : 23.04.10
�ֱ� ���� ���� : ������ ���ݰ� �⺻ ���� �и�
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        /// <summary> �� ���� </summary>
        public Define.Charcter MyClass { get; protected set; }

        /// <summary> �⺻ ���� </summary>
        [Header("Stat Ratio")]
        protected float _basicAttackRatio;
        /// <summary> ��ų ������ ��� </summary>
        protected float _basicSkillRatio;
        /// <summary> �����۰� ������ ����� �⺻ ���� ������ ��� </summary>
        protected float _attackDMGRatio;
        /// <summary> �����۰� ������ ����� ��ų ������ ��� </summary>
        protected float _skillDMGRatio;

        
        [Header("Animation")]
        protected Character4D _char4D;

        /// <summary> animation ���� �� �ʱ�ȭ </summary>
        protected override void init()
        {
            _char4D = GetComponent<Character4D>();
            _char4D.AnimationManager.SetState(CharacterState.Idle);
        }
        
        /// <summary> ���� ����, ���� ���� ���� </summary>
        public virtual void IsAttack()
        {
            //��Ÿ�� ���� �ƴ� ��
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStats[MyClass];

                //��Ÿ� ���� ���Ͱ� ������ ��
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.AttackRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.Attack();
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
                Characterstat stat = GameManager.InGameData.CharacterStats[MyClass];

                //��Ÿ� ���� ���Ͱ� ������ ��
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.Attack();
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
        enum PlayerState
        {
            Idle,
            Move,
            Attack,
            Skill,
        }

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
            SeeDirection(dir);
            _char4D.AnimationManager.SetState(CharacterState.Run);
        }
        /// <summary>
        /// ���̽�ƽ ���� ����
        /// </summary>
        public void StopMove()
        {
            _state = PlayerState.Idle;
            _char4D.AnimationManager.SetState(CharacterState.Idle);
        }
        #endregion Move

        #region TargetSelect
        /// <summary> ���� ����� ���� ��ȯ </summary>
        protected MonsterController NearMoster()
        {
            //���� �ʿ� �����ϴ� �ʵ� �޾ƿ�
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

        #region AnimationDirection
        /// <summary>
        /// ��� ��ġ�� ���� �����¿� �� ���� ������ �������� �ִϸ��̼� ������
        /// </summary>
        /// <param name="targetPos">��� ��ġ</param>
        protected void SeeTarget(Vector3 targetPos) => SeeDirection((targetPos - transform.position).normalized);

        /// <summary>
        /// ���̽�ƽ ���⿡ ���� �����¿� �� ���� ������ �������� �ִϸ��̼� ������
        /// </summary>
        /// <param name="dir">���̽�ƽ ����(normalized)</param>
        protected void SeeDirection(Vector2 dir)
        {
            Vector2 resultDir;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                resultDir = (dir.x < 0 ? Vector2.left : Vector2.right);
            else
                resultDir = (dir.y < 0 ? Vector2.down : Vector2.up);

            _char4D.SetDirection(resultDir);
        }
        #endregion AnimationDirection

        #region StatUpdate
        /// <summary> ������ �� ���� ���¿� ���� ���� ��� </summary>
        protected void StatUpdate()
        {
            _attackDMGRatio = _basicAttackRatio;
            _skillDMGRatio = _basicSkillRatio;
        }
        #endregion StatUpdate
    }
}
