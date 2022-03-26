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
namespace Network
{
    ///<summary>
    ///服务端网络服务类
    ///<summary>
public class ServerUdpNetworkService : MonoSingleton<ServerUdpNetworkService>
{
        public byte[] data;
        public UdpClient udpServive;
        public event EventHandler<MessageArrivedEventArgs> MessageArrived;
        private Thread threadReceive;
        private List<IPEndPoint> allClientEP;
        public override void Init()
        {
            base.Init();
            allClientEP = new List<IPEndPoint>();
            
        }
        //创建Socket对象，（由登录窗口传递服务端地址。端口）
        public void Initialized(string serverIP, int serverPort)
        {
            DontDestroyOnLoad(gameObject);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            udpServive = new UdpClient(serverEP);
            //创建服务端终结点
            threadReceive = new Thread(ReceiveCharMessage);
            threadReceive.Start();
            //发送上线消息
        }
        //发送数据
        public void SendChatMessage(ChatMassage msg,IPEndPoint remote)
        {
            byte[] buffer = msg.ObjectToBytes();
            udpServive.Send(buffer, buffer.Length,remote);
        }
        //接收数据
        private void ReceiveCharMessage()
        {
            while (true)
            {

                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                data = udpServive.Receive(ref remote);
                ChatMassage msg = ChatMassage.BytesToObject(data);
                //根据消息类型，执行相关逻辑
                OnMessageArried(msg, remote);
                if (MessageArrived == null)
                {
                    //下面的语句不执行，进入下一次循环
                    continue;
                }
                MessageArrivedEventArgs args = new MessageArrivedEventArgs()
                {
                    ArrivedTime = DateTime.Now,
                    Massage = msg
                };
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                {
                    MessageArrived(this, args);
                });
            }
        }
        //引发事件（1.事件参数类2.委托3.声明事件4.引发）
        //关闭释放资源
        private void OnApplicationQuit()
        {
            threadReceive.Abort();
            udpServive.Close();
        }
        private void OnMessageArried(ChatMassage msg,IPEndPoint remote)
        {
            switch (msg.Type)
            {
                case MessageType.OnLine:
                    //添加客户端
                    allClientEP.Add(remote);
                    break;
                case MessageType.OffLine:
                    //移除客户端
                    allClientEP.Remove(remote);
                    break;
                case MessageType.General:
                    allClientEP.ForEach(item => SendChatMessage(msg, item));
                    //转发
                    break;
                default:
                    break;
            }
        }
        //添加客户端
        //转发
        //移除客户端
    }
}
