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
    ///聊天消息
    ///<summary>
public class ChatMassage 
{
        public MessageType Type { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 对象转换成字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ObjectToBytes()
        {
            //对象变字节数组 string/int/boo--二进制写入器BinaryWrite--内存流MemoryStream--》byte
            using (MemoryStream stream = new MemoryStream())
            {
                //属性-写入器->内存流
                BinaryWriter writer = new BinaryWriter(stream);
                WriteString(writer, Type.ToString());
                WriteString(writer, SenderName);
                WriteString(writer, Content);
                //内存流转字节数组
                return stream.ToArray();
            }
        }
        /// <summary>
        /// 将字符串写入流中
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="str"></param>
        private void WriteString(BinaryWriter writer,string str)
        {
            if (str==null)
            {
                str = string.Empty;
            }
            //编码
            byte[] typeBTS = Encoding.Unicode.GetBytes(str);
            //写入长度
            writer.Write(typeBTS.Length);
            //写入内容
            writer.Write(typeBTS);
        }
        /// <summary>
        /// 字节数组转换为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatMassage BytesToObject(byte[] bytes)
        {
            //创建聊天消息对象
            ChatMassage obj = new ChatMassage();
            //byte[]--内存流--二进制读取器-->string/int/bool
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
        /// 从流中读取字节数组
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static string ReadString(BinaryReader reader)
        {
            int typeLength = reader.ReadInt32();//从当前流中读取 4 字节有符号整数，并使流的当前位置提升 4 个字节
            byte[] typeBtd = reader.ReadBytes(typeLength);
            string strType = Encoding.Unicode.GetString(typeBtd);
            return strType;
        }
    }
}
