using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    ///<summary>
    ///
    ///<summary>
public class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
{
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance==null)
                {
                    //�ڳ���ס�������Ͳ�������
                    instance = FindObjectOfType<T>();
                    if (instance==null)
                    {
                        //�����ű���������ִ��Awake��
                        new GameObject("Singleton"+typeof(T)).AddComponent<T>();
                    }
                    else
                    instance.Init();
                }
                return instance;
            }
        }
        protected void Awake()
        {
            if (instance==null)
            {
                instance = this as T;
                Init();
            }
           
        }
        public virtual void Init()
        {

        }
    }
}
