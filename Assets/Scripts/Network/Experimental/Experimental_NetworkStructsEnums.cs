using System;
using System.Net;
using System.Threading;
using UnityEngine;

public class Experimental_NetworkStructsEnums{}

public struct ServerConnection
{
    public ServerConnectionInfo serverConnectionInfo;
    public Thread thread;
    public UDP_Client udpClient;
    public TCP_Client tcpClient;
    public Action<string> callback;

    public ServerConnection(ServerConnectionInfo serverConnectionInfo, Thread thread, UDP_Client udpClient, TCP_Client tcpClient, Action<string> callback)
    {
        this.serverConnectionInfo = serverConnectionInfo;
        this.thread = thread;
        this.udpClient = udpClient;
        this.tcpClient = tcpClient;
        this.callback = callback;
    }
}

public struct ServerConnectionInfo
{
    public IPAdress iPAdress;
    public int port;
    public ConnexionType connexionType;

    public ServerConnectionInfo (IPAdress iPAdress, int port, ConnexionType connexionType)
    {
        this.iPAdress = iPAdress;
        this.port = port;
        this.connexionType = connexionType;
    }

    public static bool operator ==(ServerConnectionInfo sci1, ServerConnectionInfo sci2)
    {
        return sci1.iPAdress.ToString() == sci2.iPAdress.ToString() &&
            sci1.port == sci2.port &&
            sci1.connexionType == sci2.connexionType;
    }

    public static bool operator !=(ServerConnectionInfo sci1, ServerConnectionInfo sci2)
    {
        return sci1.iPAdress.ToString() != sci2.iPAdress.ToString() ||
            sci1.port != sci2.port ||
            sci1.connexionType != sci2.connexionType;
    }
}


public enum ConnexionType
{
    UDP,
    TCP
}

public struct IPAdress
{
    public int byte1;
    public int byte2;
    public int byte3;
    public int byte4;

    public IPAdress(int byte1, int byte2, int byte3, int byte4)
    {
        this.byte1 = byte1;
        this.byte2 = byte2;
        this.byte3 = byte3;
        this.byte4 = byte4;
    }

    public override string ToString()
    {
        return byte1.ToString() + "." + byte2.ToString() + "." + byte3.ToString() + "." + byte4.ToString();
    }
}