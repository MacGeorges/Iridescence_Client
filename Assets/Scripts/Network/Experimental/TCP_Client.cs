using System;
using System.Net.Sockets;

public class TCP_Client
{
    public void ConnectAndListen(object connectionInfo)
    {
        ServerConnectionInfo serverConnectionInfo = (ServerConnectionInfo)connectionInfo;

        try
        {
            // Create a TcpClient.

            // Prefer a using declaration to ensure the instance is Disposed later.
            using TcpClient client = new TcpClient(serverConnectionInfo.iPAdress.ToString(), serverConnectionInfo.port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("message");

            // Get a client stream for reading and writing.
            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", "message");

            // Receive the server response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);

            // Explicit close is not necessary since TcpClient.Dispose() will be
            // called automatically.
            // stream.Close();
            // client.Close();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

        Console.WriteLine("\n Press Enter to continue...");
        Console.Read();
    }
}
