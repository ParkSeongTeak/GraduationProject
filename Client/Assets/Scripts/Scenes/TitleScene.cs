/*
�ۼ��� : �̿쿭
�ۼ��� : 23.03.29
�ֱ� ���� ���� : 23.04.05
�ֱ� ���� ���� : Ÿ��Ʋ �� ����
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class TitleScene : MonoBehaviour
    {
        void Start()
        {
            GameManager.UI.ShowSceneUI<UI_TitleScene>();
        }
    }
}
