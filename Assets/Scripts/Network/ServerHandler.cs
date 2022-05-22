using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ServerHandler
{
    public NetworkUser user;
    public StateObject state;

    public bool authenticated;

    public void Send(RequestType requestType, string data = "")
    {
        NetworkRequest newRequest = new NetworkRequest();
        newRequest.sender = ClientManager.instance.user;
        newRequest.requestType = requestType;
        newRequest.serializedRequest = data;

        byte[] byteData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(newRequest));

        state.workSocket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), state.workSocket);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            int bytesSent = state.workSocket.EndSend(ar);
            Debug.Log("Sent " + bytesSent + " bytes to server.");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void Receive()
    {
        try
        {
            state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        Debug.Log("ReceiveCallback");
        try
        {
            string message = string.Empty;

            int bytesRead = state.workSocket.EndReceive(ar);

            if (bytesRead > 0)
            {
                message = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);

                Debug.Log("Server message : " + message);

                if (message.Contains("<EOF>"))
                {
                    // All the data has been read from the
                    // client.

                    string[] messages = message.Split("<EOF>");

                    //Debug.Log("sub messages : " + messages.Length);

                    foreach (string subMessage in messages)
                    {
                        string cleanSubMessage = subMessage.Replace("<EOF>", "");

                        if (string.IsNullOrEmpty(cleanSubMessage)) { continue; }

                        //Debug.Log("cleanSubMessage : " + cleanSubMessage);

                        HandleRequest(JsonUtility.FromJson<NetworkRequest>(cleanSubMessage));
                    }
                    state.buffer = new byte[StateObject.BufferSize];
                    state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);

                    return;
                }

                // Not all data received. Get more.  
                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);

            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void HandleRequest(NetworkRequest request)
    {
        Debug.Log("Recieved request " + request.requestType);
        Debug.Log("Serialized Data : " + request.serializedRequest);
        switch (request.requestType)
        {
            case RequestType.ping:
                //Send(RequestType.ping);
                break;
            case RequestType.login:
                user.userID = request.sender.userID;
                authenticated = true;
                Send(RequestType.login);
                break;
            case RequestType.regionChange:
                break;
            case RequestType.objectUpdate:
                Debug.Log("objectUpdate : " + JsonUtility.FromJson<ObjectRequest>(request.serializedRequest));
                EnvironmentManager.instance.HandleObjectRequest(JsonUtility.FromJson<ObjectRequest>(request.serializedRequest));
                break;
            case RequestType.chat:
                ChatManager.instance.ChatRecieved(request);
                break;
            case RequestType.playerAction:
                break;
            default:
                break;
        }
    }
}
