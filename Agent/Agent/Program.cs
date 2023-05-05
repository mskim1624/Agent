using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            Agent server = new Agent(tcpPort: 7777);
            server.Start();

            Console.WriteLine("Press Enter to exit.\n");
            Console.ReadLine();

            server.Stop();
            Environment.Exit(0);
        }

       
    }
}
