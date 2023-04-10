using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_GameOver : UI_PopUp
    {
        enum Buttons
        {
            TitleBtn,
            RetryBtn,
        }
        enum Images
        {
            WinImg,
            DefeatImg,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<Image>(typeof(Images));

            ButtonBind();
            ImageBind();
        }

        #region Button
        void ButtonBind()
        {
            BindEvent(GetButton((int)Buttons.TitleBtn).gameObject, Btn_GoToTitle);
            BindEvent(GetButton((int)Buttons.RetryBtn).gameObject, Btn_Retry);
        }

        void Btn_GoToTitle(PointerEventData evt)
        {
            SceneManager.LoadScene(Define.Scenes.Title);
        }

        void Btn_Retry(PointerEventData evt)
        {
            SceneManager.LoadScene(Define.Scenes.Game);
        }
        #endregion Button

        #region Image
        /// <summary>
        /// ���߿� Win Defeat �̹��� ����� �׶� �̹��� ���� ����� ������ ���� ����
        /// ������ �׳� Set True False�� �����ϰ� ��Ÿ��
        /// </summary>
        void ImageBind()
        {
            if(GameManager.InGameData.CurrState() == Define.State.Win)
            {
                GetImage((int)Images.WinImg).gameObject.SetActive(true);
                GetImage((int)Images.DefeatImg).gameObject.SetActive(false);

            }
            else
            {

                GetImage((int)Images.WinImg).gameObject.SetActive(false);
                GetImage((int)Images.DefeatImg).gameObject.SetActive(true);
            }
        }

        #endregion

        public override void ReOpen()
        {

        }
    }
}
