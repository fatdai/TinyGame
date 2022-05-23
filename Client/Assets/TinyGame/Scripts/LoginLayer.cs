using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;
using System.IO;
using LitJson;
using System.Threading;

public class Log
{
    const string DEFAULT_TAG = "MyLog";

    public static void Info(string msg)
    {
        Info(DEFAULT_TAG, msg, true);
    }

    public static void Info(string tag, string msg, bool bShowThreadId = false)
    {
        Debug.Log($"[<thread>-{Thread.CurrentThread.ManagedThreadId}] [{tag}] {msg}");
    }
}


public class LoginLayer : MonoBehaviour
{
    [SerializeField]
    InputField m_UserName, m_Pwd;

    public void Btn_Login() {

        NetGlobal.instance.Connect(()=> {

            string username = m_UserName.text.Trim();
            string pwd = m_Pwd.text.Trim();

            // 开始读写数据
            LoginData data = new LoginData
            {
                UserName = username,
                Pwd = pwd
            };
          
            NetGlobal.instance.Send(CS_ID.LOGIN,data);
        });
    }
}
