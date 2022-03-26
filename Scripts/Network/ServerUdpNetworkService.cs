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
    ///��������������
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
        //����Socket���󣬣��ɵ�¼���ڴ��ݷ���˵�ַ���˿ڣ�
        public void Initialized(string serverIP, int serverPort)
        {
            DontDestroyOnLoad(gameObject);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            udpServive = new UdpClient(serverEP);
            //����������ս��
            threadReceive = new Thread(ReceiveCharMessage);
            threadReceive.Start();
            //����������Ϣ
        }
        //��������
        public void SendChatMessage(ChatMassage msg,IPEndPoint remote)
        {
            byte[] buffer = msg.ObjectToBytes();
            udpServive.Send(buffer, buffer.Length,remote);
        }
        //��������
        private void ReceiveCharMessage()
        {
            while (true)
            {

                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                data = udpServive.Receive(ref remote);
                ChatMassage msg = ChatMassage.BytesToObject(data);
                //������Ϣ���ͣ�ִ������߼�
                OnMessageArried(msg, remote);
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
            threadReceive.Abort();
            udpServive.Close();
        }
        private void OnMessageArried(ChatMassage msg,IPEndPoint remote)
        {
            switch (msg.Type)
            {
                case MessageType.OnLine:
                    //��ӿͻ���
                    allClientEP.Add(remote);
                    break;
                case MessageType.OffLine:
                    //�Ƴ��ͻ���
                    allClientEP.Remove(remote);
                    break;
                case MessageType.General:
                    allClientEP.ForEach(item => SendChatMessage(msg, item));
                    //ת��
                    break;
                default:
                    break;
            }
        }
        //��ӿͻ���
        //ת��
        //�Ƴ��ͻ���
    }
}
