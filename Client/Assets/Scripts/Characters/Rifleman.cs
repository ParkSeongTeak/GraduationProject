using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client
{
    /// <summary>
    /// �����ø� �⺻ ���� : ���Ÿ�, ���� <br/>
    /// �����ø� ��ų : ���Ÿ�, ����
    /// </summary>
    public class Rifleman : PlayerController
    {
        protected override void init()
        {
            MyClass = Define.Charcter.Rifleman;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// ������ġ

            _attackDMGRatio = 1.5f;
            _skillDMGRatio = 5;
        }
    }
}
