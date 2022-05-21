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

                //Debug.Log("Server message : " + message);

                HandleRequest(JsonUtility.FromJson<NetworkRequest>(message));

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
        //Debug.Log("Recieved " + request.requestType);
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
