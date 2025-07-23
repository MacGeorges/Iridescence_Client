using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Experimental_ServersManager : MonoBehaviour
{

    private void Awake()
    {
        ServerConnections.InitThreads();
        ConnectToServer(new ServerConnectionInfo(new IPAdress(127, 0, 0, 1), 80, ConnexionType.UDP));
    }

    private void OnApplicationQuit()
    {
        ServerConnections.StopThreads();
    }

    public void ConnectToServer(ServerConnectionInfo serverConnectionInfo)
    {
        ServerConnections.ConnectToServer(serverConnectionInfo);
    }

    public void DisconnectFromServer(ServerConnectionInfo serverConnectionInfo)
    {
        ServerConnections.DisconnectFromServer(serverConnectionInfo);
    }
}