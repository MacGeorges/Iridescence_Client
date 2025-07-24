using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDP_Client
{
    private UdpClient client;
    private IPEndPoint remoteEP;
    private Action<string> callback;

    public void Init(ServerConnection serverConnection)
    {
        client = new UdpClient();
        IPAddress address = IPAddress.Parse(serverConnection.serverConnectionInfo.iPAdress.ToString());
        remoteEP = new IPEndPoint(address, serverConnection.serverConnectionInfo.port);
        callback = serverConnection.callback;
        
        //We don't need that as we send a message to the server before all, establishing connection.
        //For listeners only, the connection needs to be explicitely started
        //client.Connect(remoteEP);
    }

    public void Receive()
    {
        Debug.Log("Start Listening");

        //Send a login request to the server before listening
        Send();

        while (true)
        {
            Debug.Log("Listening " + client + " - " +remoteEP);
            byte[] data = client.Receive(ref remoteEP);
            string message = Encoding.ASCII.GetString(data);

            Debug.Log("Message : " + message);

            if (message.Contains("<EOR>"))
            {
                callback.Invoke(message.Replace("<EOR>", ""));
            }
        }
    }

    public void Send()
    {
        // Sends a test message
        Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

        client.Send(sendBytes, sendBytes.Length, remoteEP);
    }

    public void Disconnect()
    {
        client.Close();
    }


    public void ConnectAndListen(object connectionInfo)
    {
        Debug.LogError("This shouldn't be called!");

        /*ServerConnectionInfo serverConnectionInfo = (ServerConnectionInfo)connectionInfo;

        // This constructor arbitrarily assigns the local port number.
        //UdpClient udpClient = new UdpClient(11000);
        try
        {
            udpClient.Connect(serverConnectionInfo.iPAdress.ToString(), serverConnectionInfo.port);

            // Sends a message to the host to which you have connected.
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

            udpClient.Send(sendBytes, sendBytes.Length);

            // Sends a message to a different host using optional hostname and port parameters.
            UdpClient udpClientB = new UdpClient();
            udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Console.WriteLine("This is the message you received " +
                                         returnData.ToString());
            Console.WriteLine("This message was sent from " +
                                        RemoteIpEndPoint.Address.ToString() +
                                        " on their port number " +
                                        RemoteIpEndPoint.Port.ToString());

            udpClient.Close();
            udpClientB.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }*/
    }
}
