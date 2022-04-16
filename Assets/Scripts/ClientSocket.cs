using System;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;

public class ClientSocket
{
    public bool connected = false;
    private byte[] m_recvBuff;
    private AsyncCallback m_recvCb;
    private Queue<string> m_msgQueue = new Queue<string>();
    private Socket m_socket;

    private Socket init()
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_recvBuff = new byte[0x4000];
        m_recvCb = new AsyncCallback(RecvCallBack);
        return clientSocket;
    }

    // 连接服务器
    public void Connect(string host, int port)
    {
        if (m_socket == null)
            m_socket = init();
        try
        {
            Debug.Log("connect: " + host + ":" + port);
            m_socket.SendTimeout = 3;
            m_socket.Connect(host, port);
            connected = true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    // 发送消息
    public void SendData(byte[] bytes)
    {
        NetworkStream netstream = new NetworkStream(m_socket);
        netstream.Write(bytes, 0, bytes.Length);
    }

    // 尝试接收消息（每帧调用）
    public void BeginReceive()
    {
        m_socket.BeginReceive(m_recvBuff, 0, m_recvBuff.Length, SocketFlags.None, m_recvCb, this);
    }

    // 当收到服务器的消息时会回调这个函数
    private void RecvCallBack(IAsyncResult ar)
    {
        var len = m_socket.EndReceive(ar);
        byte[] msg = new byte[len];
        Array.Copy(m_recvBuff, msg, len);
        var msgStr = System.Text.Encoding.UTF8.GetString(msg);
        // 将消息塞入队列中
        m_msgQueue.Enqueue(msgStr);
        // 将buffer清零
        for (int i = 0; i < m_recvBuff.Length; ++i)
        {
            m_recvBuff[i] = 0;
        }
    }

    // 从消息队列中取出消息
    public string GetMsgFromQueue()
    {
        if (m_msgQueue.Count > 0)
            return m_msgQueue.Dequeue();
        return null;
    }

    // 关闭Socket
    public void CloseSocket()
    {
        Debug.Log("close socket");
        try
        {
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            m_socket = null;
            connected = false;
        }
    }
}