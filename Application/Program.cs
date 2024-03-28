using System;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;

namespace ConnectionCatcher
{
    class ConnectionCatcher
    {

        static void Main(string[] args)
        {
            // Setting up the TcpListener to catch incoming traffic
            TcpListener Listener = new TcpListener(IPAddress.Any, 80);
            Listener.Start();

            Console.WriteLine("Listening for incoming connections...");

            while (true)
            {
                // Start accepting incoming connections
                TcpClient client = Listener.AcceptTcpClient();

                // Handle connections in seperate threads
                System.Threading.Thread thread = new System.Threading.Thread(() => HandleConnections(client));
                thread.Start();
            }
        }

        static void HandleConnections(TcpClient client)
        {
            // Get the clients IP and port information
            IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            string clientIP = clientEndPoint.Address.ToString();
            int clientPort = clientEndPoint.Port;
            Console.WriteLine($"New connection from: {clientIP}:{clientPort}");

            // Printing out the traffic information
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string dataReceived = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Data received: {dataReceived}");

            // Close the connection
            client.Close();
        }
    }
}