using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;
using MyTinyGame;
using System.IO;

public class LoginLayer : MonoBehaviour
{
    [SerializeField]
    InputField m_UserName, m_Pwd;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_Login() {
        string username = m_UserName.text.Trim();
        string pwd = m_Pwd.text.Trim();

        // socket
        m_Socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        // connect
        IPAddress ipAddr;
        if(!IPAddress.TryParse("127.0.0.1", out ipAddr))
        {
            Debug.LogError("地址错误!");
            return;
        }
        m_Socket.Connect(new IPEndPoint(ipAddr, 9001));

        // 开始读写数据
        LoginData data = new LoginData { 
            UserName = username,
            Pwd = pwd
        };
        byte[] dataBin = CodecHelper.Serialze(data);
        byte[] buf = new byte[4 + dataBin.Length];
        BufferUtils.WriteUShort(CS_ID.LOGIN, buf, 0);
        BufferUtils.WriteUShort((ushort)dataBin.Length, buf, 2);
        BufferUtils.WriteByteArray(dataBin, buf, 4);
        m_Socket.Send(buf);

        // 开始接收数据
        m_Socket.BeginReceive(m_Buffer, m_UsedCount, RemainCount, SocketFlags.None, CB_Receive, null);

    }

    // 
    byte[] m_Buffer = new byte[1024 * 4];
    int m_UsedCount;
    Socket m_Socket;
    public int RemainCount
    {
        get
        {
            return m_Buffer.Length - m_UsedCount;
        }
    }


    void CB_Receive(IAsyncResult ar)
    {
        try
        {
            //count是接收数据的大小
            int count = m_Socket.EndReceive(ar);
            //数据处理
            m_UsedCount += count;
            ProcessData();
            //继续接收	
            m_Socket.BeginReceive(m_Buffer, m_UsedCount, RemainCount, SocketFlags.None, CB_Receive, null);
        }
        catch (Exception e)
        {
            Debug.Log("CB_Receive Error!");
            Debug.Log($"msg:{e.Message}");
            m_Socket.Close();
        }
    }

    void ProcessData() {
        if (m_UsedCount < 4)
        {
            return;
        }
        ushort cmd = BitConverter.ToUInt16(m_Buffer, 0);
        ushort len = BitConverter.ToUInt16(m_Buffer, 2);
        Debug.Log($"cmd:{cmd},len:{len}");

        // 
        if (m_UsedCount < 4 + len)
        {
            return;
        }

        // decode msg
        byte[] msgBuffer = new byte[len];
        Array.Copy(m_Buffer, 4, msgBuffer, 0, len);

        // decode msgBuffer
        HandlerMsg(cmd, msgBuffer);

        //
        int moveCount = m_UsedCount - 4 - len;
        Array.Copy(m_Buffer, 4 + len, m_Buffer, 0, moveCount);
        m_UsedCount = moveCount;

        // 处理粘包
        if (m_UsedCount > 0)
        {
            ProcessData();
        }
    }

    private void HandlerMsg(ushort cmd, byte[] msgBuffer)
    {

    }
}
