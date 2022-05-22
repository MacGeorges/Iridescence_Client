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

    public void Send(NetworkRequest newRequest)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(newRequest) + "<EOF>");

        state.workSocket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), state.workSocket);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            int bytesSent = state.workSocket.EndSend(ar);
            //Debug.Log("Sent " + bytesSent + " bytes to server.");
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
        try
        {
            string message = string.Empty;

            int bytesRead = state.workSocket.EndReceive(ar);

            if (bytesRead > 0)
            {
                message = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);

                if (message.Contains("<EOF>"))
                {
                    string[] messages = message.Split("<EOF>");

                    foreach (string subMessage in messages)
                    {
                        string cleanSubMessage = subMessage.Replace("<EOF>", "");

                        if (string.IsNullOrEmpty(cleanSubMessage)) { continue; }

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
        //Debug.Log("Recieved request " + request.requestType);
        //Debug.Log("Serialized Data : " + request.serializedRequest);
        switch (request.requestType)
        {
            case RequestType.ping:
                //Send(RequestType.ping);
                break;
            case RequestType.login:
                user.userID = request.sender.userID;
                authenticated = true;
                request.sender = ClientManager.instance.user;
                Send(request);
                break;
            case RequestType.regionChange:
                break;
            case RequestType.objectUpdate:
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
