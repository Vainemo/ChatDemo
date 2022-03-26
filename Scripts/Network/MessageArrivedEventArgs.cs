using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Network
{
    ///<summary>
    ///消息到达事件参数类
    ///<summary>
public class MessageArrivedEventArgs:EventArgs
{
        public ChatMassage Massage;
        public DateTime ArrivedTime;

}
}
