using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Experimental_ServersManager : MonoBehaviour
{

    private void Awake()
    {
        ServerConnections.Init();
        //For testing
        ConnectToServer(new ServerConnectionInfo(new IPAdress(127, 0, 0, 1), 11000, ConnexionType.UDP), TestCallback);
    }

    private void TestCallback(string message)
    {
        Debug.Log("Receiving message : " + message);
        NetworkRequest networkRequest = JsonUtility.FromJson<NetworkRequest>(message);

        ServerHandler.HandleRequest(networkRequest);
    }

    private void OnApplicationQuit()
    {
        ServerConnections.StopAllConnections();
    }

    public void ConnectToServer(ServerConnectionInfo serverConnectionInfo, Action<string> callback)
    {
        ServerConnections.ConnectToServer(serverConnectionInfo, callback);
    }

    public void DisconnectFromServer(ServerConnectionInfo serverConnectionInfo)
    {
        ServerConnections.DisconnectFromServer(serverConnectionInfo);
    }
}