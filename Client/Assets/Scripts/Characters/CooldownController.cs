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

        public void Clear()
        {
            for(int i = 0; i< _cooldowns.Length; i++) _cooldowns[i] = 0;
        }

        /// <summary>
        /// ���� ��ٿ� ���� ��ȯ
        /// </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        /// <summary>
        /// ��ų ��ٿ� ���� ��ȯ
        /// </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;


        public IEnumerator CooldownCoroutine()
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
