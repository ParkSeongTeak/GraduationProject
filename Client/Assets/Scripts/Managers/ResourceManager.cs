/*
���� �ۼ�
�ۼ��� : 23.03.29
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ResourceManager
    {
        /// <summary> Resources.Load�� �ҷ�����
        /// <para> ĳ���� �� ���ΰ�?? </para> </summary>
        public T Load<T>(string path) where T : Object
        {
            /*
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = GameManager.Pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }
            */

            return Resources.Load<T>(path);
        }

        /// <summary> GameObject ���� </summary>
        public GameObject Instantiate(string path, Transform parent = null) => Instantiate<GameObject>(path, parent);
        /// <summary> T type object ���� </summary>
        public T Instantiate<T>(string path, Transform parent = null) where T : UnityEngine.Object
        {
            T prefab = Load<T>($"Prefabs/{path}");
            if(prefab == null)
            {
                Debug.LogError($"Failed to load prefab : {path}");
                return null;
            }

            T instance = UnityEngine.Object.Instantiate<T>(prefab, parent);
            instance.name = prefab.name;

            return instance;
        }

        public void Clear()
        {

        }
    }
}
