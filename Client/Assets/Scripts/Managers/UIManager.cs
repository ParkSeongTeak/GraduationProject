using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        /// popup ui ���� ������ ���� ����
        /// </summary>
        int _order = 1;
        /// <summary>
        /// ���� Scene�� �⺻ UI
        /// </summary>
        UI_Scene _sceneUI = null;

        /// <summary>
        /// UI�� �θ� 
        /// </summary>
        GameObject _root = null;
        public GameObject Root
        {
            get
            {
                if (_root == null)
                {
                    if ((_root = GameObject.Find("UIRoot")) == null)
                        _root = new GameObject { name = "UIRoot" };
                }
                return _root;
            }
        }

        /// <summary>
        /// game object�� canvas �Ӽ� �ο�, ���� ����
        /// </summary>
        /// <param name="go">canvas �Ӽ��� �ִ� ���� ������Ʈ</param>
        /// <param name="sort">canvas ���� ����(popup->true, scene->false)</param>
        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            if (sort)
                canvas.sortingOrder = _order++;
            else
                canvas.sortingOrder = 0;
        }

        /// <summary>
        /// Scene �⺻ UI ����
        /// </summary>
        /// <typeparam name="T">UI_Scene�� ��ӹ��� �� Scene�� UI</typeparam>
        /// <param name="name">Scene UI �̸�, null�̸� T �̸�</param>
        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if(string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
            T sceneUI = Util.GetOrAddComponent<T>(go);
            _sceneUI = sceneUI;

            go.transform.SetParent(Root.transform);

            return sceneUI;
        }

        /// <summary>
        /// Pop Up UI ����
        /// </summary>
        /// <typeparam name="T">UI_PopUp�� ��ӹ��� Pop up UI</typeparam>
        /// <param name="name">Pop Up UI �̸�, null�̸� T �̸�</param>
        public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
        {
            if(string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = GameManager.Resource.Instantiate($"UI/PopUp/{name}");
            T popupUI = Util.GetOrAddComponent<T>(go);
            _popupStack.Push(popupUI);

            go.transform.SetParent(Root.transform);

            return popupUI;
        }

        /// <summary>
        /// Ư�� pop up UI �ݱ�, stack�� ���� ���� �ƴϸ� ���� X
        /// </summary>
        /// <param name="popup">�ݰ��� �ϴ� popup</param>
        public void ClosePopUpUI(UI_PopUp popup)
        {
            if(_popupStack.Count <= 0) return;

            if (_popupStack.Peek() != popup)
            {
                Debug.LogError("Pop Up doesn't match. Can't close pop up.");
                return;
            }

            ClosePopUpUI();
        }
        /// <summary>
        /// ���� ���� pop up UI �ݱ�
        /// </summary>
        public void ClosePopUpUI()
        {
            if (_popupStack.Count <= 0) return;

            UI_PopUp popup = _popupStack.Pop();
            UnityEngine.Object.Destroy(popup.gameObject);
            _order--;
        }

        /// <summary>
        /// ��� pop up UI �ݱ�
        /// </summary>
        public void CloseAllPopUpUI()
        {
            while (_popupStack.Count > 0)
                ClosePopUpUI();
        }

        /// <summary>
        /// UI �ʱ�ȭ
        /// </summary>
        public void Clear()
        {
            CloseAllPopUpUI();
            _sceneUI = null;
        }
    }
}