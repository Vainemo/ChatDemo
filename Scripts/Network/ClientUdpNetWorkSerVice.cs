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
    ///�ͻ������������
    ///<summary>
public class ClientUdpNetWorkSerVice :MonoSingleton<ClientUdpNetWorkSerVice>
{
       public UdpClient udpServive;
        public event EventHandler<MessageArrivedEventArgs> MessageArrived;
        private Thread threadReceive;
        //����Socket���󣬣��ɵ�¼���ڴ��ݷ���˵�ַ���˿ڣ�
        public void OnServerInitialized(string serverIP,int serverPort)
        {
            DontDestroyOnLoad(gameObject);
            //����������ʹ�õĶ˿�
            udpServive = new UdpClient();
            //�����˽�������(û���������֣�������������Socket)
            //����������ս��
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            udpServive.Connect(serverEP);
            threadReceive = new Thread(ReceiveCharMessage);
            threadReceive.Start();
            NotifyServer(MessageType.OnLine);
            //����������Ϣ
        }
        //֪ͨ�����
        public void NotifyServer(MessageType type)
        {
            SendChatMessage(new ChatMassage() { Type = type,SenderName="SenderName", Content="StartConent"});
        }
        //��������
        public void SendChatMessage(ChatMassage msg)
        {
            byte[] buffer = msg.ObjectToBytes();
            //����ʱ���ܰ��նˣ�����Socket����ʱ�����������ӣ�
            udpServive.Send(buffer, buffer.Length);
        }
        //��������
        private void ReceiveCharMessage()
        {
            while (true)
            {

                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpServive.Receive(ref remote);
                ChatMassage msg = ChatMassage.BytesToObject(data);
                if (MessageArrived == null)
                {
                    //�������䲻ִ�У�������һ��ѭ��
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
        //�����¼���1.�¼�������2.ί��3.�����¼�4.������
        //�ر��ͷ���Դ
        private void OnApplicationQuit()
        {
            NotifyServer(MessageType.OffLine);
            threadReceive.Abort();
            udpServive.Close();
        }
    }
}
