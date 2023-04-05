using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class CooldownController
    {
        /// <summary>
        /// �÷��̾��� ��Ÿ�� ����
        /// </summary>
        float[] _cooldowns = new float[2];
        int[] _maxCooldowns = new int[2];

        public void Clear()
        {
            for(int i = 0; i< _cooldowns.Length; i++) _cooldowns[i] = 0;
        }

        /// <summary>
        /// ���� ��ٿ� ���� ��ȯ
        /// </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        public void SetAttackCool(int cool) => _cooldowns[0] = _maxCooldowns[0] = cool;
        public float GetAttackCoolRate()
        {
            if (_maxCooldowns[0] == 0) return 1;
            return (_maxCooldowns[0] - _cooldowns[0]) / _maxCooldowns[0];
        }

        /// <summary>
        /// ��ų ��ٿ� ���� ��ȯ
        /// </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;
        public void SetSkillCool(int cool) => _cooldowns[1] = _maxCooldowns[1] = cool;
        public float GetSkillCoolRate()
        {
            if (_maxCooldowns[1] == 0) return 1;
            return (_maxCooldowns[1] - _cooldowns[1]) / _maxCooldowns[1];
        }


        public IEnumerator CooldownCoroutine()
        {
            while(GameManager.InGameData.Stat() == Define.State.Play)
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
