using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary> ������ �⺻ ���� : ���Ÿ� ���� </summary>
    public class Wizard : PlayerController
    {
        /// <summary> ������ ��ų : ���Ÿ� ����(Ÿ�� �߽�) </summary>
        public override void IsSkill()
        {
            //��Ÿ�� ���� �ƴ� ��
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                //��Ÿ� ���� ���Ͱ� ������ ��
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.Jab();
                    GenerateTargetArea(1, mon.transform.position).SetDamage(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
            }
            else
                Debug.Log("skill cool");
        }

        protected override void init()
        {
            base.init();
            MyClass = Define.Charcter.Wizard;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// ������ġ

            _attackDMGRatio = 1;
            _skillDMGRatio = 2;
        }
    }
}