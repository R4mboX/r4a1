using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SimpleTCP : MonoBehaviour
{
    [Header("Storage")]
    public byte[] ReceivedData = new byte[2] { 0x00, 0x00 };

    [Header("Settings")]
    public int Port = 1337;

    [Header("Status")]
    public string Status1 = "No Connection";
    public string Status2 = "";
    public string Status3 = "";
    public string Error1 = "";
    public string Error2 = "";

    // Host
    private TcpListener H_Listener;
    private Thread H_ListenerThread;
    private TcpClient H_Client;
    private Thread H_ClientThread;

    // Client
    private TcpClient C_Client;
    private Thread C_ClientThread;

    // Shared
    private NetworkStream N_Stream;

    public void StartHost()
    {
        try
        {
            H_Listener = new TcpListener(IPAddress.Any, Port);
            H_Listener.Start();

            H_ListenerThread = new Thread(ListenForClients);
            H_ListenerThread.IsBackground = true;
            H_ListenerThread.Start();

            Status1 = "Host";
            Status2 = "Port: " + Port;
            Status3 = "Waiting for Client";
            Error1 = "";
            Error2 = "";
        }
        catch (Exception ex)
        {
            Error1 = "Host Error";
            Error2 = ex.Message;
        }
    }

    private void ListenForClients()
    {
        try
        {
            H_Client = H_Listener.AcceptTcpClient();
            Status3 = "Client connected";

            // Start Thread for Client
            H_ClientThread = new Thread(() => HandleClient());
            H_ClientThread.IsBackground = true;
            H_ClientThread.Start();

            N_Stream = H_Client.GetStream();

            H_ListenerThread.Abort();
        }
        catch (Exception ex)
        {
            Error1 = "Client Connection";
            Error2 = ex.Message;
        }
    }

    private void HandleClient()
    {
        NetworkStream clientStream = H_Client.GetStream();
        try
        {
            ReceiveData(clientStream);
        }
        catch
        {
            Status3 = "Waiting for Client"; // DISCONNECTED CLIENT
            H_ListenerThread.Start();
            H_ClientThread.Abort();
        }
    }

    public void Connect(string A_IP, int A_Port)
    {
        try
        {
            C_Client = new TcpClient(A_IP, A_Port);
            N_Stream = C_Client.GetStream();

            Status1 = "Client";
            Status2 = "Remote IP: " + A_IP + ", Port: " + A_Port;

            Error1 = "";
            Error2 = "";

            // Start ReceiveData as Thread
            C_ClientThread = new Thread(() => ReceiveData());
            C_ClientThread.IsBackground = true;
            C_ClientThread.Start();
        }
        catch
        {
            Error1 = "No Connection";
            Error2 = "Error connecting to Host";
        }
    }

    public void SendData(byte[] A_Data)
    {
        if (N_Stream.CanWrite)
        {
                N_Stream.Write(A_Data, 0, A_Data.Length);
        }
        else
        {
            Error1 = "Error during SendData()";
            Error2 = "";
            Disconnect();
        }
    }

    private void ReceiveData(NetworkStream inputStream = null)
    {
        try
        {
            inputStream = inputStream ?? N_Stream;
            byte[] T_Buffer = new byte[2];
            int T_BufferSize;

            while ((T_BufferSize = inputStream.Read(T_Buffer, 0, T_Buffer.Length)) != 0)
            {
                ReceivedData = new byte[2];

                Array.Copy(T_Buffer, 0, ReceivedData, 0, T_BufferSize);
                Debug.Log("Received: 0x" + T_Buffer[0].ToString("X2") + ", 0x" + T_Buffer[1].ToString("X2"));
            }
        }
        catch (Exception ex)
        {
            Error1 = "Error at ReceiveData";
            Error2 = ex.Message;
            Disconnect();
        }
    }

    public void Disconnect()
    {
        if (Status1 == "Host")
        {
            Status1 = "No Connection";
            Status2 = "";
            Status3 = "";

            H_ClientThread.Abort();
            H_ListenerThread.Abort();
            H_Client.Close();
            H_Listener.Stop();
            N_Stream.Close();
        }

        if (Status1 == "Client")
        {
            Status1 = "No Connection";
            Status2 = "";
            Status3 = "";

            C_ClientThread.Abort();
            C_Client.Close();
            N_Stream.Close();
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}