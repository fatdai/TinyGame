using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


// 玩家信息
public class UserInfo
{
    public int UserId { get; set; }
}

public class NetGlobal : MonoBehaviour
{
    List<Action> m_LstActions = new List<Action>();
    object _locker = new object();

    Socket m_Socket;

    public static NetGlobal instance;
    public static Socket Socket { get { return instance.m_Socket; } }

    // 
    byte[] m_Buffer = new byte[1024 * 4];
    int m_UsedCount;
    public int RemainCount
    {
        get
        {
            return m_Buffer.Length - m_UsedCount;
        }
    }

    UserInfo m_UserInfo = new UserInfo();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);    
    }

    void Update()
    {
        lock (_locker)
        {
            for(int i = 0; i < m_LstActions.Count; ++i)
            {
                m_LstActions[i]();
            }
            m_LstActions.Clear();
        }
    }

    public void AddAction(Action action) {
        lock (_locker)
        {
            m_LstActions.Add(action);
        }
    }

    public void Connect(Action callback)
    {
        // socket
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // connect
        IPAddress ipAddr;
        if (!IPAddress.TryParse("127.0.0.1", out ipAddr))
        {
            Log.Info("地址错误!");
            return;
        }
        var ipEndPoint = new IPEndPoint(ipAddr, 9001);
        m_Socket.BeginConnect(ipEndPoint, (ar) => {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Log.Info($"连接服务器成功! {socket.RemoteEndPoint}");

            // 开始接收数据
            m_Socket.BeginReceive(m_Buffer, m_UsedCount, RemainCount, SocketFlags.None, CB_Receive, null);

            AddAction(callback);
        }, m_Socket);
    }

    public  void Send<T>(ushort cmd,T t)
    {
        m_Socket.Send(CodecHelper.EncodeMsg(cmd, t));
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
            Log.Info("CB_Receive Error!");
            Log.Info($"msg:{e.Message}");
            m_Socket.Close();
        }
    }

    void ProcessData()
    {
        if (m_UsedCount < 4)
        {
            return;
        }
        ushort cmd = BitConverter.ToUInt16(m_Buffer, 0);
        ushort len = BitConverter.ToUInt16(m_Buffer, 2);
        Log.Info($"cmd:{cmd},len:{len}");

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
        Log.Info($"cmd:{cmd}");
        switch (cmd)
        {
            case SC_ID.RES_LOGIN:
                Res_LoginData resLoginData = CodecHelper.DecodeMsg<Res_LoginData>(msgBuffer);
                Log.Info($"userId:{resLoginData.UserId}");
                m_UserInfo.UserId = resLoginData.UserId;
                AddAction(() => {
                    // 显示匹配界面
                    AppNav.instance.ShowMatchLayer();
                });
                break;
            case SC_ID.RES_MATCH:
                Res_MatchData resMatchData = CodecHelper.DecodeMsg<Res_MatchData>(msgBuffer);
                Log.Info("匹配成功...");
                AddAction(() => {
                    AppNav.instance.ShowGame();
                });
                break;
            default:
                break;
        }
    }
}
