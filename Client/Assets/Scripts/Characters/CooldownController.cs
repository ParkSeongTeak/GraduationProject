/*
�ۼ��� : �̿쿭
�ۼ��� : 23.04.05
�ֱ� ���� ���� : 23.04.10
�ֱ� ���� ���� : �ּ� �� region �߰�
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// �÷��̾� ��Ÿ�� ���� Ŭ����, InGameDataManager���� �ν��Ͻ� ����
    /// </summary>
    public class CooldownController
    {
        /// <summary> �÷��̾��� ���� ���� ��Ÿ�� ���� </summary>
        float[] _cooldowns = new float[2];
        /// <summary> �÷��̾� ����� ��Ÿ��, ��ư ���� ���� </summary>
        int[] _maxCooldowns = new int[2];

        /// <summary> ��Ÿ�� ���� �ʱ�ȭ </summary>
        public void Clear()
        {
            for(int i = 0; i< _cooldowns.Length; i++) _cooldowns[i] = 0;
        }

        #region BasicAttack
        /// <summary> ���� ���� ����(��Ÿ�� �ƴ�) ��ȯ </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        /// <summary> ���� �� ��Ÿ�� ���� </summary>
        public void SetAttackCool(int cool) => _cooldowns[0] = _maxCooldowns[0] = cool;
        /// <summary> ���� ���� ��Ÿ�� ���� ��ȯ </summary>
        public float GetAttackCoolRate()
        {
            if (_maxCooldowns[0] == 0) return 1;
            return (_maxCooldowns[0] - _cooldowns[0]) / _maxCooldowns[0];
        }
        #endregion BasicAttack

        #region Skill
        /// <summary> ��ų ���� ����(��Ÿ�� �ƴ�) ��ȯ </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;
        /// <summary> ��ų �� ��Ÿ�� ���� </summary>
        public void SetSkillCool(int cool) => _cooldowns[1] = _maxCooldowns[1] = cool;
        /// <summary> ���� ��ų ��Ÿ�� ���� ��ȯ </summary>
        public float GetSkillCoolRate()
        {
            if (_maxCooldowns[1] == 0) return 1;
            return (_maxCooldowns[1] - _cooldowns[1]) / _maxCooldowns[1];
        }
        #endregion Skill

        public IEnumerator CooldownCoroutine()
        {
            while(GameManager.InGameData.CurrState == Define.State.Play)
            {
                for(int i = 0;i < 2;i++)
                    if (_cooldowns[i] > 0)
                    {
                        _cooldowns[i] -= 0.1f;
                        if (_cooldowns[i] <= 0)
                            _cooldowns[i] = 0;
                    }
                
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
