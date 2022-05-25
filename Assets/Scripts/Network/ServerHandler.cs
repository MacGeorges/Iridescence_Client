using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ServerHandler
{
    public NetworkUser user;
    IPEndPoint remoteEP;

    public bool authenticated;

    public void StartListening()
    {
        remoteEP = new IPEndPoint(user.userIP, user.userPort);
        Debug.Log("EndPoint created : " + remoteEP);

        //IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000); // endpoint where server is listening
        //client.Connect(ep);

        NetworkRequest request = new NetworkRequest();
        request.sender = ClientManager.instance.user;
        request.requestType = RequestType.login;
        request.serializedRequest = string.Empty;

        Send(request);

        while (true)
        {
            byte[] data = AsynchronousClient.client.Receive(ref remoteEP);

            string message = Encoding.ASCII.GetString(data);

            //Debug.Log("receive data from " + remoteEP.ToString() + " : " + message);

            if (message.Contains("<EOR>"))
            {

                message = message.Replace("<EOR>", "");

                HandleRequest(JsonUtility.FromJson<NetworkRequest>(message));
            }
        }
    }

    public void Send(NetworkRequest request)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(request) + "<EOR>");

        //Debug.Log("Sending message to server : " + JsonUtility.ToJson(request));

        AsynchronousClient.client.Send(byteData, byteData.Length, remoteEP);
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
