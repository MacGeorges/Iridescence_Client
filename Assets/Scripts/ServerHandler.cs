using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ServerHandler
{
    //public Socket socket;
    public StateObject state;

    public void Send(string data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);

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
        try
        {
            string response = string.Empty;

            int bytesRead = state.workSocket.EndReceive(ar);

            if (bytesRead > 0)
            {
                response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);

                Debug.Log("Server message : " + response);

                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
