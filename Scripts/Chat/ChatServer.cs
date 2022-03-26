using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using System.IO;
using System;
using UnityEngine.UI;

namespace Network
{
    ///<summary>
    ///
    ///<summary>
public class ChatServer : MonoBehaviour
{
        private void Start()
        {
            ServerUdpNetworkService.Instance.MessageArrived += DisplayMessage;
        }

        private void DisplayMessage(object sender, MessageArrivedEventArgs e)
        {
            GetComponentInChildren<Text>().text +=e.Massage.Type+"\t";
        }
    }
}
