using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Network
{
    ///<summary>
    ///��Ϸ���
    ///<summary>
public class GameMain : MonoBehaviour
{
        private void Start()
        {
            transform.FindChildByName("ClientButton").GetComponent<Button>().onClick.AddListener(OnEnterClientButtonClick);
            transform.FindChildByName("ServerButton").GetComponent<Button>().onClick.AddListener(OnEnterServerButtonClick);
        }

        private void OnEnterServerButtonClick()
        {
            string serverIP = transform.FindChildByName("ServerIP").GetComponent<InputField>().text;
            string serverPort = transform.FindChildByName("ServerPort").GetComponent<InputField>().text;
            int port = int.Parse(serverPort);
            Network.ServerUdpNetworkService.Instance.Initialized(serverIP, port);
            SceneManager.LoadScene(2);
        }

        //����������ͻ��˰�ťʱִ��
        private void OnEnterClientButtonClick()
        {
            string serverIP = transform.FindChildByName("ClientIP").GetComponent<InputField>().text;
            string serverPort = transform.FindChildByName("ClientPort").GetComponent<InputField>().text;
            //�ɼ�������֤
            //�Ȳ�д��ͬ��
            int port = int.Parse(serverPort);
            ClientUdpNetWorkSerVice.Instance.OnServerInitialized(serverIP, port);
            SceneManager.LoadScene(1);
        }
    }
}
