using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ResourceManager
    {


        //Dictionary<string,Object> ��ư��ư ĳ���� ���⼭

        Dictionary<Define.Item, Item> �κ��丮;



        public void init()
        {

        }
        public T Load<T>(string path) where T : Object
        {
            //Ǯ�� ���Ѵٰ� �ߴ���?
            GameManager.�ǳ����ʿ���();

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

        public void Clear()
        {

        }
    }
}
