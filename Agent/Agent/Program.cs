using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HL7Server
{
    class Program
    {
        static async Task Main1(string[] args)
        {
            int port = 12345;

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server is listening on port " + port);

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("New client connected: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                // Handle the client connection asynchronously
                Task.Run(() => HandleClient(client));
            }
        }

        static async Task HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // Convert the received data to a string
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received data from client: " + receivedData);

                    // Check if the received data contains the desired string
                    if (receivedData.Contains("vCheck200"))
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string responseMessage = $"MSH|^~\\&|HL7Server|HL7Client|HL7Server|HL7Client|{timestamp}||ACK|1|P|2.3||||0||ASCII\r\nMSA|AA|1|||\r\n";

                        // Convert the response message to a byte array
                        byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);

                        // Send the response to the client
                        await stream.WriteAsync(responseData, 0, responseData.Length);

                        // Reset the buffer for the next message
                        buffer = new byte[1024];
                    }
                }
                catch (IOException)
                {
                    // The client has disconnected
                    Console.WriteLine("Client has disconnected");
                    break;
                }
            }

            client.Close();
            Console.WriteLine("Connection with client closed");
        }
    }
}
