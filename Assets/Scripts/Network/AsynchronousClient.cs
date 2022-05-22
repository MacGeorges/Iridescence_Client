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
    public const int BufferSize = 1024;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousClient
{
    public static void StartClient()
    {
        try
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            Socket client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

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
            Socket client = (Socket)ar.AsyncState;

            UnityEngine.Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());

            if (!string.IsNullOrEmpty(client.RemoteEndPoint.ToString()))
            {

                ServerHandler tmpSH = new ServerHandler();

                tmpSH.user = new NetworkUser();
                tmpSH.user.userType = UserType.server;

                tmpSH.state = new StateObject();
                tmpSH.state.workSocket = client;

                ServersManager.instance.connectedServers.Add(tmpSH);

                tmpSH.Receive();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
    }
}