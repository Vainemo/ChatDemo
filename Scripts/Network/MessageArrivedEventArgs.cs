using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Network
{
    ///<summary>
    ///��Ϣ�����¼�������
    ///<summary>
public class MessageArrivedEventArgs:EventArgs
{
        public ChatMassage Massage;
        public DateTime ArrivedTime;

}
}
