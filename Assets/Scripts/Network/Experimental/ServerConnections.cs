using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

//Create one thread per connection

class ServerConnections
{
    public static List<ServerConnection> connections;

    public static void Init()
    {
        connections = new List<ServerConnection>();
    }

    public static void ConnectToServer(ServerConnection serverConnection)
    {
        Thread thread = null;

        UDP_Client udpClient = null;
        TCP_Client tcpClient = null;

        switch (serverConnection.connexionType)
        {
            case ConnexionType.UDP:
                udpClient = new UDP_Client();
                serverConnection.udpClient = udpClient;
                udpClient.Init(serverConnection);
                thread = new Thread(udpClient.Receive);
                break;
            case ConnexionType.TCP:
                tcpClient = new TCP_Client();
                serverConnection.tcpClient = tcpClient;
                tcpClient.Init(serverConnection);
                thread = new Thread(tcpClient.Receive);
                break;
        }

        serverConnection.thread = thread;
        thread.Name = serverConnection.networkUser.userIP + " listener";

        connections.Add(serverConnection);
        thread.Start();
    }

    public static void DisconnectFromServer(ServerConnection serverConnection)
    {
        if (serverConnection.thread != null)
        {
            Debug.Log("Disconnecting " + serverConnection.networkUser.userIP);

            //Maybe create parent class to avoid that
            switch (serverConnection.connexionType)
            {
                case ConnexionType.UDP:
                    serverConnection.udpClient.Disconnect();
                    break;
                case ConnexionType.TCP:
                    serverConnection.tcpClient.Disconnect();
                    break;
            }

            serverConnection.thread.Abort();
        }
    }

    public static void StopAllConnections()
    {
        foreach (ServerConnection connection in connections)
        {
            DisconnectFromServer(connection);
        }

        Debug.Log("All thread Stopped");
    }
}