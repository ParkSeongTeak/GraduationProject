using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class UI_PopUp : UI_Base
    {
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, true);
        }

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
