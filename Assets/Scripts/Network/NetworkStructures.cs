using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NetworkStructures : MonoBehaviour { }

[Serializable]
public struct NetworkRequest
{
    public NetworkUser sender;
    public RequestType requestType;
    public string serializedRequest;

    public NetworkRequest(NetworkUser newSender, RequestType newRequestType, string requestContent)
    {
        sender = newSender;
        requestType = newRequestType;
        serializedRequest = requestContent;
    }

    public static bool operator ==(NetworkRequest nr1, NetworkRequest nr2)
    {
        return (nr1.sender == nr2.sender &&
            nr1.requestType == nr2.requestType &&
            nr1.serializedRequest == nr2.serializedRequest);
    }

    public static bool operator !=(NetworkRequest nr1, NetworkRequest nr2)
    {
        return (nr1.sender != nr2.sender ||
            nr1.requestType != nr2.requestType ||
            nr1.serializedRequest != nr2.serializedRequest);
    }
}

[Serializable]
public enum RequestMethod
{
    Get,
    Post
}

public struct ServerConnection
{
    //public ServerConnectionInfo serverConnectionInfo;
    public NetworkUser networkUser;
    public Thread thread;
    public ConnexionType connexionType;
    public UDP_Client udpClient;
    public TCP_Client tcpClient;

    public ServerConnection(NetworkUser networkUser, Thread thread, ConnexionType connexionType, UDP_Client udpClient, TCP_Client tcpClient)
    {
        this.networkUser = networkUser;
        this.thread = thread;
        this.connexionType = connexionType;
        this.udpClient = udpClient;
        this.tcpClient = tcpClient;
    }

    public static bool operator ==(ServerConnection sc1, ServerConnection sc2)
    {
        return sc1.networkUser == sc2.networkUser &&
            sc1.thread == sc2.thread &&
            sc1.connexionType == sc2.connexionType &&
            sc1.udpClient == sc2.udpClient &&
            sc1.tcpClient == sc2.tcpClient;
    }

    public static bool operator !=(ServerConnection sc1, ServerConnection sc2)
    {
        return sc1.networkUser != sc2.networkUser ||
            sc1.thread != sc2.thread ||
            sc1.connexionType != sc2.connexionType ||
            sc1.udpClient != sc2.udpClient ||
            sc1.tcpClient != sc2.tcpClient;
    }
}

public enum ConnexionType
{
    UDP,
    TCP
}

[Serializable]
public struct NetworkUser
{
    public UserType userType;
    public string userID;
    public long userIP;
    public int userPort;
    public bool isAuthenticated;

    public static bool operator ==(NetworkUser c1, NetworkUser c2)
    {
        return (c1.userIP == c2.userIP && c1.userPort == c2.userPort);
    }

    public static bool operator !=(NetworkUser c1, NetworkUser c2)
    {
        return (c1.userIP != c2.userIP || c1.userPort != c2.userPort);
    }
}

[Serializable]
public enum UserType
{
    server,
    client,
    bot
}

[Serializable]
public enum RequestType
{
    ping,
    login,
    regionChange,
    objectUpdate,
    chat,
    playerAction
}

[Serializable]
public struct ObjectRequest
{
    public ObjectRequestType requestType;
    public RegionElement element;

    public ObjectRequest(ObjectRequestType newRequestType, RegionElement newElement)
    {
        requestType = newRequestType;
        element = newElement;
    }
}

[Serializable]
public enum ObjectRequestType
{
    add,
    remove,
    update
}

[Serializable]
public struct PlayerActionRequest
{
    public SerializableTransform spatialData;

    public PlayerActionRequest(SerializableTransform newspatialData)
    {
        spatialData = newspatialData;
    }
}