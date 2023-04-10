/*
�ۼ��� : �̿쿭
�ۼ��� : 23.03.31
�ֱ� ���� ���� : 23.03.31
�ֱ� ���� ���� : �⺻ UI �ý��� ����
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public abstract class UI_Base : MonoBehaviour
    {
        /// <summary>
        /// ������ ���� ������Ʈ��
        /// </summary>
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

        /// <summary>
        /// UI ���� �ʱ�ȭ
        /// </summary>
        public abstract void Init();
        private void Start()
        {
            Init();
        }
        /// <summary>
        /// ������ T type object�� _objects dictionary�� ����
        /// </summary>
        /// <typeparam name="T">�ش� Ÿ��</typeparam>
        /// <param name="type">�ش� Ÿ�� ���� ���� enum(�� UI���� ����)</param>
        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects.Add(typeof(T), objects);

            for(int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Util.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.LogError($"Failed to bind : {names[i]} on {gameObject.name}");
            }
        }

        /// <summary>
        /// bind�� object���� ���ϴ� object ���
        /// </summary>
        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            if (_objects.TryGetValue(typeof(T), out objects) == false)
                return null;

            return objects[idx] as T;
        }
        #region Get_Override
        protected GameObject GetGameObject(int idx) => Get<GameObject>(idx);
        protected TMP_Text GetText(int idx) => Get<TMP_Text>(idx);

        protected Image GetImage(int idx) => Get<Image>(idx);
        protected Button GetButton(int idx) => Get<Button>(idx);
        #endregion Get_Override
    
        /// <summary>
        /// �ش� game object�� �̺�Ʈ �Ҵ�
        /// </summary>
        /// <param name="action">�Ҵ��� �̺�Ʈ</param>
        /// <param name="type">�̺�Ʈ �߻� ����</param>
        public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

            switch(type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
                case Define.UIEvent.DragEnd:
                    evt.OnDragEndHandler -= action;
                    evt.OnDragEndHandler += action;
                    break;
            }
        }
    }
}
