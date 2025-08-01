using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor.PackageManager;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.tvOS;

public class TCP_Client
{
    TcpClient client;
    NetworkStream stream;
    private Action<string> callback;

    public void Init(ServerConnection serverConnection)
    {
        //client = new TcpClient(serverConnection.networkRequest.sender.userIP.ToString(), serverConnection.networkRequest.sender.userPort);
        client = new TcpClient(serverConnection.networkUser.userIP.ToString(), serverConnection.networkUser.userPort);
        stream = client.GetStream();
        //callback = serverConnection.callback;
    }

    public void Receive()
    {
        while (true)
        {
            byte[] data = new byte[256];

            int bytes = stream.Read(data, 0, data.Length);
            String message = Encoding.ASCII.GetString(data, 0, bytes);

            if (message.Contains("<EOR>"))
            {
                callback.Invoke(message.Replace("<EOR>", ""));
            }
        }
    }

    public void Send(NetworkRequest request)
    {
        byte[] data = Encoding.ASCII.GetBytes(JsonUtility.ToJson(request) + "<EOR>");

        stream.Write(data, 0, data.Length);
    }

    public void Disconnect()
    {
        client.Close();
    }

    public void ConnectAndListen(object connectionInfo)
    {
        // Translate the passed message into ASCII and store it as a Byte array.
        Byte[] data = System.Text.Encoding.ASCII.GetBytes("message");

        // Get a client stream for reading and writing.
        NetworkStream stream = client.GetStream();

        // Send the message to the connected TcpServer.
        stream.Write(data, 0, data.Length);

        Console.WriteLine("Sent: {0}", "message");

        // Receive the server response.

        while (true)
        {
            // Buffer to store the response bytes.
            data = new Byte[256];

            // Read the first batch of the TcpServer response bytes.
            //int bytes = stream.Read(data, 0, data.Length);
            //string responseData = Encoding.ASCII.GetString(data, 0, bytes);

            string message = Encoding.ASCII.GetString(data);

            Debug.Log("Message : " + message);

            if (message.Contains("<EOR>"))
            {
                ServerHandler.HandleRequest(JsonUtility.FromJson<NetworkRequest>(message.Replace("<EOR>", "")));
            }
        }
        
        // Explicit close is not necessary since TcpClient.Dispose() will be
        // called automatically.
        // stream.Close();
        // client.Close();
    }
}
