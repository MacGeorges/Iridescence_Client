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

    public static void ConnectToServer(ServerConnectionInfo serverConnectionInfo, Action<string> callback)
    {
        ServerConnections w = new ServerConnections();
        Thread thread = null;

        UDP_Client udpClient = null;
        TCP_Client tcpClient = null;

        ServerConnection serverConnection = new ServerConnection(serverConnectionInfo, null, null, null, callback);

        switch (serverConnectionInfo.connexionType)
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
        thread.Name = serverConnectionInfo.iPAdress + " listener";

        connections.Add(serverConnection);
        thread.Start();
    }

    public static void DisconnectFromServer(ServerConnectionInfo serverConnectionInfo)
    {
        ServerConnection connection = connections.Find(c => c.serverConnectionInfo == serverConnectionInfo);

        if (connection.thread != null)
        {
            Debug.Log("Disconnecting " + connection.serverConnectionInfo.iPAdress);

            switch (connection.serverConnectionInfo.connexionType)
            {
                case ConnexionType.UDP:
                    connection.udpClient.Disconnect();
                    break;
                case ConnexionType.TCP:
                    connection.tcpClient.Disconnect();
                    break;
            }

            connection.thread.Abort();
        }
    }

    public static void StopAllConnections()
    {
        foreach (ServerConnection connection in connections)
        {
            DisconnectFromServer(connection.serverConnectionInfo);
        }

        Debug.Log("All thread Stopped");
    }
}