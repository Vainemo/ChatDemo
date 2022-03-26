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
                    //在场景住根据类型查找引用
                    instance = FindObjectOfType<T>();
                    if (instance==null)
                    {
                        //创建脚本对象（立即执行Awake）
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
