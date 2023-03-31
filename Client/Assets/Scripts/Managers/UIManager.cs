using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class UIManager
    {
        /// <summary>
        /// �˾� UI ������ ���� stack
        /// </summary>
        Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
        /// <summary>
        /// ���� Scene�� �⺻ UI
        /// </summary>
        UI_Scene _sceneUI = null;

        /// <summary>
        /// UI�� �θ� 
        /// 
        /// GameObject root ������ ���� 
        /// if(root == null)
        ///     GameObject root = GameObject.Find("UIRoot");
        /// �ϸ� �� �� ���� ������...�ϴ� ������ �ѹ� �غ�. ���� ��û ���� �����Ұ� ������ �Ź� Find�� ���� ���� �����ޱ�......
        /// </summary>
        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("UIRoot");
                if (root == null)
                    root = new GameObject { name = "UIRoot" };
                return root;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go">canvas �Ӽ��� �ִ� ���� ������Ʈ</param>
        /// <param name="sort">canvas ���� ����(popup->true, scene->false)</param>
        public void SetCanvas(GameObject go, bool sort = true)
        {

        }
    }
}