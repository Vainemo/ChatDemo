using ns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    ///<summary>
    ///线程交叉访问助手类
    ///<summary>
    public class ThreadCrossHelper : MonoSingleton<ThreadCrossHelper>
    {
        public override void Init()
        {
            base.Init();
            //actionList = new List<DelayedItem>();
            DontDestroyOnLoad(gameObject);
        }
        public ThreadCrossHelper()
        {
            actionList = new List<DelayedItem>();
        }
        class DelayedItem
        {
            public Action CurrentAction { get; set; }
            public DateTime Time { get; set; }
        }
        private List<DelayedItem> actionList;
        private void Update()
        {
            lock (actionList)
            {
                for (int i = actionList.Count - 1; i >= 0; i--)
                {
                    if (actionList[i].Time <= DateTime.Now)
                    {
                        actionList[i].CurrentAction();
                        //移除
                        actionList.RemoveAt(i);
                    }
                }
            }
        }
        public void ExecuteOnMainThread(Action action, float delay = 0)
        {
            lock(actionList)
            {
                var item = new DelayedItem()
                {
                    CurrentAction = action,
                    Time = DateTime.Now.AddSeconds(delay)
                };
                actionList.Add(item);

            }
        }
}
}
