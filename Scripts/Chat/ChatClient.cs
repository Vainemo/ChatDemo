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
public class ChatClient : MonoBehaviour
{
        private InputField messageInput;
        private InputField nameInput;
        private  Text chatText;
        private ScrollRect scrollRect;
        private void Start()
        {
            transform.FindChildByName("Send").GetComponent<Button>().onClick.AddListener(OnSendMessageButtonClick);
            messageInput= transform.FindChildByName("ChatInputField").GetComponent<InputField>();
            nameInput = transform.FindChildByName("NameInput").GetComponent<InputField>();
            chatText= transform.FindChildByName("ChatText").GetComponent<Text>();
            scrollRect= transform.FindChildByName("TextShowPanel").GetComponent<ScrollRect>();
            //注册消息到达时事件
            ClientUdpNetWorkSerVice.Instance.MessageArrived += DisplayMessage;
        }
        //显示数据
        private void DisplayMessage(object sender, MessageArrivedEventArgs e)
        {
            string addText = "\n  " + "<color=red>" + e.Massage.SenderName + "</color>: " + e.Massage.Content;
            chatText.text += addText;
            messageInput.text = "";
            messageInput.ActivateInputField();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            Canvas.ForceUpdateCanvases();
        }
        //发送信息
        private void OnSendMessageButtonClick()
        {
            ChatMassage msg = new ChatMassage()
            {
                Type = MessageType.General,
                SenderName = nameInput.text,
                Content=messageInput.text,
            };

            ClientUdpNetWorkSerVice.Instance.SendChatMessage(msg);
        }
    }
}
