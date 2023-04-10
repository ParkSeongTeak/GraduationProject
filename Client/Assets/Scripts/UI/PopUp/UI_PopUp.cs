/*
�ۼ��� : �̿쿭
�ۼ��� : 23.03.29
�ֱ� ���� ���� : 23.04.05
�ֱ� ���� ���� : popup ui ��Ȱ��ȭ(ReOpen) �Լ� �߰�
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class UI_PopUp : UI_Base
    {
        /// <summary> ���� ���� </summary>
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, true);
        }

        /// <summary> pop up �ݱ� </summary>
        public virtual void ClosePopUpUI()
        {
            GameManager.UI.ClosePopUpUI(this);
        }

        /// <summary>
        /// ��Ȱ��ȭ�� UI �ٽ� Ȱ��ȭ �� ȣ��
        /// </summary>
        public abstract void ReOpen();
    }
}
