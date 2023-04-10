using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class UI_GetItem : UI_PopUp
    {
        /// <summary> ��ȭ ��ġ ���� �� �ݹ� </summary>
        public static Action OnMoneyChangedAction { get; private set; }
        
        enum Buttons
        { 
            CloseBtn,
            BuyBtn,
        }
        enum ItemTexts
        {
            Item0,
            Item1,
            Item2,
            Item3,
            Item4,
            Item5,
            Item6,
            Item7,
        }

        /// <summary> UI ���� ���� - bind, �ؽ�Ʈ �ʱ�ȭ ���� </summary>
        public override void Init()
        {
            base.Init();

            //������Ʈ ���ε�
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(ItemTexts));

            //��ư�� �̺�Ʈ ����
            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, Btn_Close);
            BindEvent(GetButton((int)Buttons.BuyBtn).gameObject, Btn_Buy);

            OnMoneyChangedAction = Buy_ActiveControl;
            Buy_ActiveControl();

            //������ �ؽ�Ʈ �ʱ�ȭ
            for (int i = 0 ; i < GameManager.InGameData.MyInventory.Count; i++)
                GetText(i).text = GameManager.InGameData.MyInventory[i].Name;
        }

        /// <summary> ��Ȱ��ȭ ���¿��� �ٽ� �� �� ȣ�� </summary>
        public override void ReOpen()
        {
            OnMoneyChangedAction = Buy_ActiveControl;
            Buy_ActiveControl();
        }

        #region Buttons
        /// <summary> �ݱ� ��ư - �ؽ�Ʈ ������Ʈ ��Ȱ��ȭ </summary>
        void Btn_Close(PointerEventData evt)
        {
            OnMoneyChangedAction = null;
            GameManager.UI.ClosePopUpUI(this);
        }
        
        /// <summary> �����ݿ� ���� ���� ��ư Ȱ��ȭ/��Ȱ��ȭ </summary>
        void Buy_ActiveControl()
        {
            GetButton((int)Buttons.BuyBtn).gameObject.SetActive(GameManager.InGameData.CanBuyItem);
            //GetButton((int)Buttons.BuyBtn).interactable = GameManager.InGameData.Money >= GameManager.InGameData.ItemCost;
        }
        /// <summary> ������ ���� ��ư </summary>
        void Btn_Buy(PointerEventData evt)
        {
            if(GameManager.InGameData.CanBuyItem)
            {
                GameManager.InGameData.Money -= GameManager.InGameData.ItemCost;

                ///������� ������ �������� Ȯ���ϰ� �ٲ����

                GameManager.InGameData.AddRandomItem();
                GetText(GameManager.InGameData.MyInventory.Count - 1).text = GameManager.InGameData.MyInventory[GameManager.InGameData.MyInventory.Count - 1].Name;
                ///

            }
        }
        #endregion Buttons
    }
}
