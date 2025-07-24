using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

public class Experimental_ServersManager : MonoBehaviour
{

    private void Start()
    {
        ServerConnections.Init();
        //For testing
        NetworkUser user = new NetworkUser();
        user.userType = UserType.server;
        user.userIP = IPAddress.Parse("127.0.0.1").Address;
        user.userPort = 11000;

        ConnectToServer(new ServerConnection(user, null, ConnexionType.UDP, null, null));
    }

    private void OnApplicationQuit()
    {
        ServerConnections.StopAllConnections();
    }

    public void ConnectToServer(ServerConnection serverConnection)
    {
        ServerConnections.ConnectToServer(serverConnection);
    }

    public void DisconnectFromServer(ServerConnection serverConnection)
    {
        ServerConnections.DisconnectFromServer(serverConnection);
    }
}