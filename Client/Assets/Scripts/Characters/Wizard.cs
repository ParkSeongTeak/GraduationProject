using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Wizard : PlayerController
    {
        /// <summary>
        /// ������ �⺻ ���� : ����
        /// </summary>
        public override void IsAttack()
        {
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[_myClass];

                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.AttackRange)
                {
                    mon.BeAttacked(AttackDMG);
                    GameManager.InGameData.Cooldown.SetAttackCool(stat.AttackCool);
                }
            }
            else
                Debug.Log("attack cool");
        }

        public override void IsSkill()
        {
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[_myClass];

                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    mon.BeAttacked(5 * AttackDMG);
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
            }
            else
                Debug.Log("skill cool");
        }

        protected override void init()
        {
            _myClass = Define.Charcter.Wizard;
            MaxHP = 98754321;
            MoveSpeed = 10.0f;
            AttackDMG = 10;
            Position = Vector2.zero;// ������ġ


        }
    }
}