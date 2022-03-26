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
    ///������Ϣ
    ///<summary>
public class ChatMassage 
{
        public MessageType Type { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// ����ת�����ֽ�����
        /// </summary>
        /// <returns></returns>
        public byte[] ObjectToBytes()
        {
            //������ֽ����� string/int/boo--������д����BinaryWrite--�ڴ���MemoryStream--��byte
            using (MemoryStream stream = new MemoryStream())
            {
                //����-д����->�ڴ���
                BinaryWriter writer = new BinaryWriter(stream);
                WriteString(writer, Type.ToString());
                WriteString(writer, SenderName);
                WriteString(writer, Content);
                //�ڴ���ת�ֽ�����
                return stream.ToArray();
            }
        }
        /// <summary>
        /// ���ַ���д������
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="str"></param>
        private void WriteString(BinaryWriter writer,string str)
        {
            if (str==null)
            {
                str = string.Empty;
            }
            //����
            byte[] typeBTS = Encoding.Unicode.GetBytes(str);
            //д�볤��
            writer.Write(typeBTS.Length);
            //д������
            writer.Write(typeBTS);
        }
        /// <summary>
        /// �ֽ�����ת��Ϊ����
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatMassage BytesToObject(byte[] bytes)
        {
            //����������Ϣ����
            ChatMassage obj = new ChatMassage();
            //byte[]--�ڴ���--�����ƶ�ȡ��-->string/int/bool
            MemoryStream stream = new MemoryStream(bytes);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                string strType = ReadString(reader);
                obj.Type = (MessageType)Enum.Parse(typeof(MessageType), strType);
                obj.SenderName = ReadString(reader);
                obj.Content = ReadString(reader);
                return obj;
            }
        }
        /// <summary>
        /// �����ж�ȡ�ֽ�����
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static string ReadString(BinaryReader reader)
        {
            int typeLength = reader.ReadInt32();//�ӵ�ǰ���ж�ȡ 4 �ֽ��з�����������ʹ���ĵ�ǰλ������ 4 ���ֽ�
            byte[] typeBtd = reader.ReadBytes(typeLength);
            string strType = Encoding.Unicode.GetString(typeBtd);
            return strType;
        }
    }
}
