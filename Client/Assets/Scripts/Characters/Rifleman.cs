/*
�ۼ��� : �̿쿭
�ۼ��� : 23.03.29
�ֱ� ���� ���� : 23.04.10
�ֱ� ���� ���� : ������ ���ݰ� �⺻ ���� �и�
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client
{
    public class Rifleman : PlayerController
    {
        /// <summary> �����ø� �⺻ ���� : ���Ÿ�, ���� </summary>
        public override void IsAttack()
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
                    _char4D.AnimationManager.ShotBow();
                    mon.BeAttacked(Mathf.RoundToInt(_attackDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(stat.AttackCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("attack cool");
        }
        /// <summary> �����ø� ��ų : ���Ÿ�, ���� </summary>
        public override void IsSkill()
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
                    _char4D.AnimationManager.ShotBow();
                    mon.BeAttacked(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("skill cool");
        }
        protected override void init()
        {
            base.init();
            MyClass = Define.Charcter.Rifleman;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// ������ġ

            _basicAttackRatio = 1.5f;
            _basicSkillRatio = 5;
            StatUpdate();
        }
    }
}
