using System;
using System.Collections;
using System.Collections.Generic;
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
}

[Serializable]
public enum RequestMethod
{
    Get,
    Post
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