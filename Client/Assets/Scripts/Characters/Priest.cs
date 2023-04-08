using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary> ���� �⺻ ���� : ����, ���� </summary>
    public class Priest : PlayerController
    {
        /// <summary>
        /// ���� ��ų : �ڽ�, ���� ����� �Ʊ� ��ȭ
        /// </summary>
        public override void IsSkill()
        {
            //��Ÿ�� ���� �ƴ� ��
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                List<PlayerController> buffTargets = new List<PlayerController>();
                buffTargets.Add(this);
                if (GameManager.InGameData.NearPlayer != null) buffTargets.Add(GameManager.InGameData.NearPlayer);

                SeeDirection(Vector2.down);
                _char4D.AnimationManager.Jab();

                //���� ����
                Debug.Log("buff");
                GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
            }
            else
                Debug.Log("skill cool");
        }

        protected override void init()
        {
            base.init();
            MyClass = Define.Charcter.Priest;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// ������ġ

            _attackDMGRatio = 1;
            _skillDMGRatio = 1.5f;
        }

    }
}
