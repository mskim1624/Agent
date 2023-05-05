using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent
{
    internal class ISmartCare10
    {
        static Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>();
        static Dictionary<string, StringBuilder> receivedData = new Dictionary<string, StringBuilder>();
        static byte[] ack = new byte[] { 0x06 };

        static async Task Main1(string[] args)
        {

            foreach (string portName in SerialPort.GetPortNames())
            {
                SerialPort serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                serialPort.DataReceived += DataReceivedHandler;
                serialPorts.Add(portName, serialPort);
                receivedData.Add(portName, new StringBuilder());
                serialPort.Open();
            }

            while (true)
            {
                foreach (string portName in serialPorts.Keys)
                {
                    StringBuilder buffer = receivedData[portName];

                    if (buffer.Length > 0)
                    {
                        string data = buffer.ToString();
                        Console.WriteLine($"Received data from {portName}: {data}");

                        // ACK 보내기
                        SerialPort serialPort = serialPorts[portName];
                        serialPort.Write(ack, 0, ack.Length);

                        buffer.Clear();
                    }
                    else
                    {
                        // 끝처리
                    }
                }
                await Task.Delay(100);
            }
        }
    
        static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // 시리얼 포트에서 들어오는 데이터를 받아들이기
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            receivedData[sp.PortName].Append(data);
            Console.WriteLine($"Received data from {sp.PortName}: {data}");
        }
    }
}
