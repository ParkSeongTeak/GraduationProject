using UnityEngine;

namespace Client
{
    public class Util
    {
        /// <summary>
        /// Game Object���� �ش� Component ��ų� ������ �߰�
        /// </summary>
        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

        /// <summary>
        /// �ش� Game Object�� �ڽ� �� T ������Ʈ�� ���� �ڽ� ���
        /// </summary>
        /// <param name="name">�ڽ��� �̸�</param>
        /// <param name="recursive">����� Ž�� ����</param>
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null) return null;

            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform child = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || child.name == name)
                    {
                        T component = child.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T child in go.GetComponentsInChildren<T>())
                    if (string.IsNullOrEmpty(name) || child.name == name)
                        return child;
            }

            return null;
        }

        /// <summary>
        /// Game Object ���� FindChild
        /// </summary>
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null) return null;
            return transform.gameObject;
        }
    }
}