using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

class ServerConnections
{
    public static Dictionary<ServerConnectionInfo, Thread> threads;

    public static void InitThreads()
    {
        threads = new Dictionary<ServerConnectionInfo, Thread>();
    }

    public static void ConnectToServer(ServerConnectionInfo serverConnectionInfo)
    {
        ServerConnections w = new ServerConnections();
        Thread newThread = null;

        switch (serverConnectionInfo.connexionType)
        {
            case ConnexionType.UDP:
                UDP_Client udp_Client = new UDP_Client();
                newThread = new Thread(udp_Client.ConnectAndListen);
                break;
            case ConnexionType.TCP:
                TCP_Client tcp_Client = new TCP_Client();
                newThread = new Thread(tcp_Client.ConnectAndListen);
                break;
        }

        newThread.Name = serverConnectionInfo.iPAdress + " listener";
        threads.Add(serverConnectionInfo, newThread);

        newThread.Start(serverConnectionInfo);
    }

    public static void DisconnectFromServer(ServerConnectionInfo serverConnectionInfo)
    {
        if (threads.TryGetValue(serverConnectionInfo, out Thread thread))
        {
            thread.Abort();
        }

    }

    public static void StopThreads()
    {
        foreach (KeyValuePair<ServerConnectionInfo, Thread> thread in threads)
        {
            Debug.Log("Aborting " + thread.Value.Name);
            thread.Value.Abort();
        }

        Debug.Log("All thread Stopped");
    }
}