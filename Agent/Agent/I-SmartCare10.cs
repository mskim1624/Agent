using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace Agent
{
    internal class I_SmartCare10
    {
      
        static SerialPort serialPort;
        static StringBuilder _receivedData = new StringBuilder();
        static byte[] ack;

        static void Main1(string[] args)
        {
            // 시리얼 포트 설정
            serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            ack = new byte[] { 0x06 };
            // 시리얼 포트 열기
            serialPort.Open();

            // 반복적으로 데이터 처리
            while (true)
            {
                if (serialPort.BytesToRead > 0)
                {
                    byte[] buffer = new byte[serialPort.BytesToRead];
                    serialPort.Read(buffer, 0, buffer.Length);
                    string data = Encoding.ASCII.GetString(buffer);

                    if (data.Contains("i-SmartCare10"))
                    {
                        // i-SmartCare10이 확인되면 메시지 출력
                        Console.WriteLine("i-SmartCare10 connected");
                    }
                }

                // 데이터 한 줄씩 받아들이기
                if (_receivedData.Length > 0)
                {
                    string data = _receivedData.ToString();
                    int endOfLine = data.IndexOf('\r');

                    if (endOfLine >= 0)
                    {
                        // 데이터의 끝을 체크하고 처리
                        string message = data.Substring(0, endOfLine);
                        Console.WriteLine("Received message: " + message);

                        // TODO: 데이터 처리

                        // ACK 보내기
                        serialPort.Write(ack, 0, ack.Length);

                        _receivedData.Remove(0, endOfLine + 1);
                    }
                }
            }
        }

        static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // 시리얼 포트에서 들어오는 데이터를 계속해서 받아들이기
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            _receivedData.Append(data);
        }
    }
}
