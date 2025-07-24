using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDP_Client
{
    public NetworkUser user
    { get; private set; }

    private UdpClient client;
    private IPEndPoint remoteEP;

    public void Init(ServerConnection serverConnection)
    {
        user = serverConnection.networkUser;
        client = new UdpClient();
        //IPAddress address = IPAddress.Parse(serverConnection.serverConnectionInfo.iPAdress.ToString());
        //remoteEP = new IPEndPoint(address, serverConnection.serverConnectionInfo.port);
        remoteEP = new IPEndPoint(user.userIP, user.userPort);

        //We don't need that as we send a message to the server before all, establishing connection.
        //For listeners only, the connection needs to be explicitely started
        //client.Connect(remoteEP);
    }

    public void Receive()
    {
        Debug.Log("Start Listening");

        NetworkRequest request = new NetworkRequest();
        request.sender = ClientManager.instance.user;
        request.requestType = RequestType.login;
        request.serializedRequest = JsonUtility.ToJson(user);

        //Send a login request to the server before listening
        Send(request);

        while (true)
        {
            byte[] data = client.Receive(ref remoteEP);
            string message = Encoding.ASCII.GetString(data);

            Debug.Log("Message : " + message);

            if (message.Contains("<EOR>"))
            {
                ServerHandler.HandleRequest(JsonUtility.FromJson<NetworkRequest>(message.Replace("<EOR>", "")));
            }
        }
    }

    public void Send(NetworkRequest request)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(JsonUtility.ToJson(request) + "<EOR>");

        client.Send(sendBytes, sendBytes.Length, remoteEP);
    }

    public void Disconnect()
    {
        client.Close();
    }
}
