using UnityEngine;
using System.Reflection;

namespace Client
{
    public class Util
    {
        /// <summary>
        /// Game Object에서 해당 Component 얻거나 없으면 추가
        /// </summary>
        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

        /// <summary>
        /// 해당 Game Object의 자식 중 T 컴포넌트를 가진 자식 얻기
        /// </summary>
        /// <param name="name">자식의 이름</param>
        /// <param name="recursive">재귀적 탐색 여부</param>
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
        /// Game Object 전용 FindChild
        /// </summary>
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null) return null;
            return transform.gameObject;
        }

        
        /// <summary>
        /// json 파일 파싱하여 오브젝트로 반환
        /// </summary>
        public static Handler ParseJson<Handler>(string path = null, string handle = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                string name = typeof(Handler).Name;
                int idx = name.IndexOf("Handler");

                path = string.Concat(name.Substring(0, idx), 's');
                handle = path.ToLower();
            }

            TextAsset jsonTxt = Resources.Load<TextAsset>($"Jsons/{path}");
            if(jsonTxt == null)
            {
                Debug.LogError($"Can't load json : {path}");
                return default(Handler);
            }
            string json = jsonTxt.text;
            return JsonUtility.FromJson<Handler>($"{{\"{handle}\" : {json} }}");
        }
    }
}