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
    //public RequestDescriptor requestDescriptor;
    public string serializedRequest;
}

[Serializable]
public struct NetworkUser
{
    public UserType userType;
    public string userID;
    public bool isAuthenticated;
}

[Serializable]
public enum UserType
{
    server,
    client,
    bot
}

[Serializable]
public struct RequestDescriptor
{
    public RequestType requestType;
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