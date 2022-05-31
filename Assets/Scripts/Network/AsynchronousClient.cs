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

        tmpSH.user = new NetworkUser();
        tmpSH.user.userType = UserType.server;

        //tmpSH.user.userIP = IPAddress.Parse("135.125.234.58").Address;
        tmpSH.user.userIP = IPAddress.Parse("127.0.0.1").Address;
        tmpSH.user.userPort = 11000;

        ServersManager.instance.connectedServers.Add(tmpSH);

        tmpSH.Init();
        tmpSH.StartListening();
    }
}