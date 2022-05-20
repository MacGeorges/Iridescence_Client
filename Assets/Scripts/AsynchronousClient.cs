using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// State object for receiving data from remote device.  
public class StateObject
{
    // Client socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 256;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousClient
{
    // The port number for the remote device.  
    private const int port = 11000;

    //private static Socket client;

    public static void StartClient()
    {
        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // The name of the
            // remote device is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            Socket client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), client);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
    }

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            //client.EndConnect(ar);

            UnityEngine.Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.

            if (!string.IsNullOrEmpty(client.RemoteEndPoint.ToString()))
            {

                ServerHandler tmpSH = new ServerHandler();
                //tmpSH.socket = client;

                tmpSH.state = new StateObject();
                tmpSH.state.workSocket = client;

                ServersManager.instance.connectedServers.Add(tmpSH);

                UnityEngine.Debug.Log("Server added to list");

                // Receive the response from the remote device.  
                //Receive(client);
                tmpSH.Receive();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
    }
}