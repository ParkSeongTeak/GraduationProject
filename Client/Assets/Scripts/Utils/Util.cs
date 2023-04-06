using UnityEngine;
using System.Reflection;

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


        /// <summary>
        /// json ���� �Ľ��Ͽ� ������Ʈ�� ��ȯ<br/>
        /// <br/>
        /// * �ʿ� Ŭ����<br/>
        /// {0} - ���� ������ ������ �ִ� Ŭ����<br/>
        /// {0}Handler - member�� �̸��� ({0}s).ToLower() �� List&lt;{0}&gt; �ʿ�<br/>
        ///<br/>
        /// * json ����<br/>
        /// Assets/Resources/Jsons/{0}s.json<br/>
        /// <br/>
        /// ex)<br/>
        /// {0} = MonsterStat�� ���,<br/>
        /// <br/>
        /// class MonsterStat - ������ ������ �ִ� Ŭ����<br/>
        /// class MonsterStatHandler { public List&lt;MonsterStat&gt; monsterstats; }<br/>
        /// Assets/Resources/Jsons/MonsterStats.json
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
            else if (string.IsNullOrEmpty(handle))
            {
                string name = typeof(Handler).Name;
                int idx = name.IndexOf("Handler");
                handle = string.Concat(name.Substring(0, idx), 's').ToLower();
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