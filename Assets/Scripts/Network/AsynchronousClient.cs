using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class AsynchronousClient
{
    public static UdpClient client;

    public static void StartClient()
    {
        client = new UdpClient();

        ServerHandler tmpSH = new ServerHandler();

        NetworkUser user = new NetworkUser();
        user.userType = UserType.server;

        //tmpSH.user.userIP = IPAddress.Parse("135.125.234.58").Address;
        user.userIP = IPAddress.Parse("127.0.0.1").Address;
        user.userPort = 11000;

        ServersManager.instance.connectedServers.Add(tmpSH);

        user.userID = ClientManager.instance.user.userID;
        tmpSH.Init(user);
        tmpSH.StartListening();
    }
}