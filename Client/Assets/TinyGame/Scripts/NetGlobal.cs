using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


// �����Ϣ
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
            Log.Info("��ַ����!");
            return;
        }
        var ipEndPoint = new IPEndPoint(ipAddr, 9001);
        m_Socket.BeginConnect(ipEndPoint, (ar) => {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Log.Info($"���ӷ������ɹ�! {socket.RemoteEndPoint}");

            // ��ʼ��������
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
            //count�ǽ������ݵĴ�С
            int count = m_Socket.EndReceive(ar);
            //���ݴ���
            m_UsedCount += count;
            ProcessData();
            //��������	
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

        // ����ճ��
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
                    // ��ʾƥ�����
                    AppNav.instance.ShowMatchLayer();
                });
                break;
            case SC_ID.RES_MATCH:
                Res_MatchData resMatchData = CodecHelper.DecodeMsg<Res_MatchData>(msgBuffer);
                Log.Info("ƥ��ɹ�...");
                AddAction(() => {
                    AppNav.instance.ShowGame();
                });
                break;
            default:
                break;
        }
    }
}
