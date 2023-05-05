using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Text;
/*
MSH(Messaging Information) 세그먼트: 메시지에 대한 정보를 담고 있습니다.
| : 필드(Field) 구분자
^~& : 컴포넌트(Component) 구분자와 서브컴포넌트(Subcomponent) 구분자를 포함하는 이스케이프 문자열(escape sequence)
Virtual HL7 Server : 송신 시스템의 이름
{mshGuid} : MSH 메시지에 대한 고유한 식별자
Instr RnD Dept : 수신 시스템의 이름
{DateTimeOffset.Now.ToString("yyyyMMddHHmmsszzz")} : 메시지 생성 일시
ACK^R01^ACK : 메시지 유형(ACK)과 서브유형(R01)을 나타냅니다.
{random.Next(1, 999999)} : 메시지 ID
P : 우선순위(normal)
2.6 : HL7 프로토콜 버전
ACK(Acknowledgment) 세그먼트: MSH 메시지의 수신 여부에 대한 응답을 담고 있습니다.
MSA|CA|{Guid.NewGuid().ToString()} : 응답 유형, 처리결과 및 메시지 ID를 포함하는 ACK 메시지 세그먼트입니다.
 */
//http://r741.realserver2.com/api/viewDB.php?page=1
namespace Agent
{
    public enum Machine
    {
        VCheck200,
        PT10,
        ISmartCare,
        None
    }

    public class Agent
    {

        Dictionary<string , SerialInfo> serialInformation = new Dictionary<string , SerialInfo>();
        byte[] ack = new byte[] { 0x06 };
        Random random = new Random();
        TcpListener tcpListener;

        public Agent(int tcpPort)
        {
            tcpListener = new TcpListener(IPAddress.Any, tcpPort);
            foreach (string portName in SerialPort.GetPortNames())
            {
                SerialPort serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                serialPort.DataReceived += SerialPortDataReceived;
                serialPort.Open();
                serialInformation.Add(portName, new SerialInfo(serialPort));
            }
        }

        public void Start()
        {
            tcpListener.Start();
            Task.Run(() => AcceptTcpClients());
            Task.Run(() => SerialPortDataSend());
        }

        private async Task AcceptTcpClients()
        {
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("AcceptTcpClient\n");
                Task.Run(() => ReceiveFromTcpClient(tcpClient));
            }
        }

        private async Task ReceiveFromTcpClient(TcpClient client)
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

                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received data from client: " + receivedData);
                    //VA20DBITG1495
                    //VA20B02VA1038
                    //VCheck 이름 입력
                    if (receivedData.Contains("VA20DBITG1495"))
                    {
                        Console.WriteLine("VCheck Only Reply\n");

                        string mshGuid = Guid.NewGuid().ToString();

                        string msh = $"MSH|^~\\&amp;|Virtual HL7 Server^{mshGuid}^GUID|Instr RnD Dept|||{DateTimeOffset.Now.ToString("yyyyMMddHHmmsszzz")}|ACK^R01^ACK|{random.Next(1, 999999)}|P|2.6";

                        string ack = $"MSA|CA|{Guid.NewGuid()}";

                        string responseMessage = $"{msh}\n\r{ack}\n\r";

                        Console.WriteLine(responseMessage);
                        byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);

                        await stream.WriteAsync(responseData, 0, responseData.Length);
                    }
                    else
                    {
                        byte[] ackData = new byte[] { 0x06 };
                        Console.WriteLine("sends an ACK message to the client.");
                        await stream.WriteAsync(ackData, 0, ackData.Length);
                    }

                    buffer = new byte[1024];
                }
                catch (IOException)
                {
                    //Console.WriteLine("Client has disconnected");
                    break;
                }
            }

            client.Close();
            Console.WriteLine("Connection with client closed");
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            serialInformation[sp.PortName].currentMessage = data;
            serialInformation[sp.PortName].message.Append(data);
            serialInformation[sp.PortName].isConnection = true;
            Console.WriteLine($"Received data from {sp.PortName}: {data}");
            if(serialInformation[sp.PortName].machine == Machine.None)
            {
                if (serialInformation[sp.PortName].Contains(""))
                    serialInformation[sp.PortName].machine = Machine.ISmartCare;
            }
        }


        private async Task SerialPortDataSend()
        {
            while (true)
            {
                foreach (string portName in serialInformation.Keys)
                {
                    string buffer = serialInformation[portName].currentMessage;

                    if (!string.IsNullOrEmpty(buffer))
                    {
                        serialInformation[portName].ACK(Machine.ISmartCare, ack);
                        
                        Console.WriteLine("sends an ACK message to the client.");
                        buffer = string.Empty;
                    }
                    else
                    {
                        if (serialInformation[portName].isConnection)
                        {

                            serialInformation[portName].isConnection = false;
                            serialInformation[portName].currentMessage = string.Empty;
                            serialInformation[portName].message.Clear();
                        }
                    }
                }
                await Task.Delay(100);
            }
        }

        public void Stop()
        {
            tcpListener.Stop();
            foreach (KeyValuePair<string, SerialInfo> item in serialInformation)
                item.Value.port.Close();
        }

    }

}