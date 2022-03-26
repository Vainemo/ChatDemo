using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Network
{
    ///<summary>
    ///游戏入口
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

        //当单击进入客户端按钮时执行
        private void OnEnterClientButtonClick()
        {
            string serverIP = transform.FindChildByName("ClientIP").GetComponent<InputField>().text;
            string serverPort = transform.FindChildByName("ClientPort").GetComponent<InputField>().text;
            //可加数据验证
            //先不写，同上
            int port = int.Parse(serverPort);
            ClientUdpNetWorkSerVice.Instance.OnServerInitialized(serverIP, port);
            SceneManager.LoadScene(1);
        }
    }
}
