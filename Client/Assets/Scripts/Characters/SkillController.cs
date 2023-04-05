using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class SkillController
    {
        /// <summary>
        /// �÷��̾��� ��Ÿ�� ����
        /// </summary>
        float[] _cooldowns = new float[2];
        /// <summary>
        /// ��ų ��Ÿ�
        /// </summary>
        int[] _ranges = new int[2];
        
        /// <summary>
        /// �÷��̾� ������ ���� ��Ÿ� �ҷ�����
        /// </summary>
        /// <param name="character"></param>
        public void LoadData(Define.Charcter character)
        {

        }

        /// <summary>
        /// ���� ��ٿ� ���� ��ȯ
        /// </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        /// <summary>
        /// ��ų ��ٿ� ���� ��ȯ
        /// </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;

        /// <summary>
        /// �⺻ ���� ��Ÿ� ��ȯ
        /// </summary>
        public int GetAttackRange() => _ranges[0];
        /// <summary>
        /// ��ų ��Ÿ� ��ȯ
        /// </summary>
        public int GetSkillRange() => _ranges[1];


        IEnumerator CooldownCoroutine()
        {
            while(GameManager.InGameData.Stat() == Define.State.Play)
            {
                for(int i = 0;i < 2;i++)
                    if (_cooldowns[i] > 0)
                    {
                        _cooldowns[i] -= 0.25f;
                        if (_cooldowns[i] <= 0)
                            _cooldowns[i] = 0;
                    }
                
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
