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
    ///客户端网络服务类
    ///<summary>
public class ClientUdpNetWorkSerVice :MonoSingleton<ClientUdpNetWorkSerVice>
{
       public UdpClient udpServive;
        public event EventHandler<MessageArrivedEventArgs> MessageArrived;
        private Thread threadReceive;
        //创建Socket对象，（由登录窗口传递服务端地址。端口）
        public void OnServerInitialized(string serverIP,int serverPort)
        {
            DontDestroyOnLoad(gameObject);
            //随机分配可以使用的端口
            udpServive = new UdpClient();
            //与服务端建立连接(没有三次握手，仅仅配置自身Socket)
            //创建服务端终结点
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            udpServive.Connect(serverEP);
            threadReceive = new Thread(ReceiveCharMessage);
            threadReceive.Start();
            NotifyServer(MessageType.OnLine);
            //发送上线消息
        }
        //通知服务端
        public void NotifyServer(MessageType type)
        {
            SendChatMessage(new ChatMassage() { Type = type,SenderName="SenderName", Content="StartConent"});
        }
        //发送数据
        public void SendChatMessage(ChatMassage msg)
        {
            byte[] buffer = msg.ObjectToBytes();
            //发送时不能绑定终端（创建Socket对象时，建立了连接）
            udpServive.Send(buffer, buffer.Length);
        }
        //接收数据
        private void ReceiveCharMessage()
        {
            while (true)
            {

                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpServive.Receive(ref remote);
                ChatMassage msg = ChatMassage.BytesToObject(data);
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
            NotifyServer(MessageType.OffLine);
            threadReceive.Abort();
            udpServive.Close();
        }
    }
}
