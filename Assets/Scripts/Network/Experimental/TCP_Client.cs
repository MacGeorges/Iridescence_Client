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
        client = new TcpClient(serverConnection.serverConnectionInfo.iPAdress.ToString(), serverConnection.serverConnectionInfo.port);
        stream = client.GetStream();
        callback = serverConnection.callback;
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

    public void Send(ServerConnectionInfo serverConnectionInfo, string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    public void Disconnect()
    {
        client.Close();
    }

    public void ConnectAndListen(object connectionInfo)
    {
        ServerConnectionInfo serverConnectionInfo = (ServerConnectionInfo)connectionInfo;

        try
        {
            // Create a TcpClient.

            // Prefer a using declaration to ensure the instance is Disposed later.
            using TcpClient client = new TcpClient(serverConnectionInfo.iPAdress.ToString(), serverConnectionInfo.port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("message");

            // Get a client stream for reading and writing.
            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", "message");

            // Receive the server response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);

            // Explicit close is not necessary since TcpClient.Dispose() will be
            // called automatically.
            // stream.Close();
            // client.Close();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

        Console.WriteLine("\n Press Enter to continue...");
        Console.Read();
    }
}
