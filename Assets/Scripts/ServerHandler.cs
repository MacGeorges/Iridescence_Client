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
        Debug.Log("Sending using Socket : " + state.workSocket);
        Debug.Log("Sending using Socket RemoteEndPoint : " + state.workSocket.RemoteEndPoint.ToString());

        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        state.workSocket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), state.workSocket);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            //state.workSocket = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
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
            // Create the state object.  
            //StateObject state = new StateObject();
            //state.workSocket = socket;

            // Begin receiving the data from the remote device.  
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

            // Retrieve the state object and the client socket
            // from the asynchronous state object.  
            //state = (StateObject)ar.AsyncState;
            //Socket client = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = state.workSocket.EndReceive(ar);

            Debug.Log("Recieved " + bytesRead + " bytes");

            if (bytesRead > 0)
            {
                response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);

                Debug.Log("response : " + response);

                // Get the rest of the data.  
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
