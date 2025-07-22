using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Experimental_ServersManager : MonoBehaviour
{
    //private List<Experimental_ServerConnection> serverConnections;

    private void Awake()
    {
        ThreadingStuff.StartThreadingStuff();
        //JoinServer(new IPAdress(127, 0, 0 ,1), ConnexionType.UDP);
    }

    private void OnApplicationQuit()
    {
        ThreadingStuff.StopThreadingStuff();
    }

    public bool JoinServer(IPAdress IPAdress, ConnexionType connexionType)
    {
        Experimental_ServerConnection newConnection = new Experimental_ServerConnection();

        Experimental_ServerConnection.Init(IPAdress, connexionType);

        //serverConnections.Add(newConnection);

        Thread listenerThread = new Thread(Experimental_ServerConnection.StartConnection);
        listenerThread.Start();

        return true;
    }
}