using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;

namespace Agent_Forms
{
    public partial class Agent : Form
    {
        List<SerialPort> portList = new List<SerialPort>();
        SerialPort currentPort;
        TaskCompletionSource<bool> messageReceivedTcs = new TaskCompletionSource<bool>();

        Dictionary<string, SerialInfo> serialInformation = new Dictionary<string, SerialInfo>();
        byte[] ack = new byte[] { 0x06 };
        Random random = new Random();
        TcpListener tcpListener;

        IPAddress ipAddress;
        int port;
        int clientsCount;
        string inputAck;

        //AutoResetEvent

        public Agent()
        {
            InitializeComponent();

            for (int i = 0; i < SerialPort.GetPortNames().Length; i++)
            {
                string name = SerialPort.GetPortNames()[i];
                PortNameBox.Items.Add(name);
                portList.Add(new SerialPort(name));
                portList[i].DataReceived += SerialPortDataReceived;
                serialInformation.Add(name, new SerialInfo(portList[i]));
                try
                {
                    portList[i].Open();
                }
                catch (Exception ex)
                {
                    // 시리얼 포트 열기 거부 예외 처리
                    Console.WriteLine("시리얼 포트 열기 거부: " + ex.Message);
                }
            }

            PortNameBox.SelectedIndex = 0;
            currentPort = portList[0];

            for (int i = 1200; i <= 9600; i *= 2)
                Box1.Items.Add(i);

            Box1.SelectedIndex = Box1.Items.Count - 1;

            Box2.Items.AddRange(Enum.GetNames(typeof(Parity)));
            Box2.SelectedIndex = 0;

            for (int i = 5; i <= 8; i++)
                Box3.Items.Add(i);

            Box3.SelectedIndex = Box3.Items.Count - 1;

            Box4.Items.AddRange(Enum.GetNames(typeof(StopBits)));
            Box4.SelectedIndex = 1;

            timer.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] bytes = { 0x1c };
            string str = Encoding.ASCII.GetString(bytes);

            StartServer();
        }
        private async void StartServer()
        {
            try
            {
                // IP 주소 가져오기
                ipAddress = GetLocalIPAddress();

                IPBox.Text = ipAddress.ToString();

                // 포트 설정
                port = int.Parse(PortBox.Text);

                // TCP 서버 열기
                tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();


                // 로그에 서버 시작 메시지 출력
                string message = $"서버 시작됨 - IP: {ipAddress}, 포트: {port}";
                AddLog(message);

                Task.Run(() => AcceptTcpClients());
                Task.Run(() => SerialPortDataSend());
            }
            catch (Exception ex)
            {
                AddLog($"서버 시작 실패 - {ex.Message}");
            }
        }
        private async Task AcceptTcpClients()
        {
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                AddLog("Connection with client opened");
                clientsCount++;
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
                    AddLog("Received data from client: " + receivedData);
                    //VA20DBITG1495
                    //VA20B02VA1038
                    //VCheck 이름 입력
                    if (receivedData.Contains("VA20DBITG1495"))
                    {
                        AddLog("VCheck Only Reply");

                        string mshGuid = Guid.NewGuid().ToString();

                        string msh = $"MSH|^~\\&amp;|Virtual HL7 Server^{mshGuid}^GUID|Instr RnD Dept|||{DateTimeOffset.Now.ToString("yyyyMMddHHmmsszzz")}|ACK^R01^ACK|{random.Next(1, 999999)}|P|2.6";

                        string ack = $"MSA|CA|{Guid.NewGuid()}";

                        string responseMessage = $"{msh}\r\n{ack}\r\n";

                        AddChat(responseMessage);

                        await messageReceivedTcs.Task;

                        byte[] responseData = Encoding.ASCII.GetBytes(inputAck);

                        AddLog("Server : " + inputAck);

                        await stream.WriteAsync(responseData, 0, responseData.Length);

                        messageReceivedTcs = new TaskCompletionSource<bool>();
                    }
                    else
                    {
                        string ackMessage = await CreateACKMessage(receivedData);

                        AddChat(ackMessage);

                        await messageReceivedTcs.Task;

                        byte[] responseData = Encoding.ASCII.GetBytes(inputAck);

                        await stream.WriteAsync(responseData, 0, responseData.Length);

                        AddLog("Server Response : " + inputAck);

                        messageReceivedTcs = new TaskCompletionSource<bool>();
                    }
                    buffer = new byte[1024];
                }
                catch (IOException)
                {
                    break;
                }
            }

            client.Close();
            clientsCount--;
            AddLog("Connection with client closed");
        }

        static async Task<string>CreateACKMessage(string message)
        {
            string[] segments = message.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string[] fields = segments[0].Split('|');
            string messageControlID = fields[9];
            string messageType = fields[8];

            //
            string msh = $"MSH|^~\\&|LIS|LAB|LABGEO PT10|LAB|{DateTimeOffset.Now.ToString("yyyyMMddHHmmsszzz")}||ACK^R22^ACK|{messageControlID}|P|2.5.1|||ER|AL||UNICODE UTF-8|||LAB-29^IHE";

            //string msh = $"MSH|^~\\&|ABL80 BASIC^309448|ABL80 BASIC^309448|||{DateTimeOffset.Now.ToString("yyyyMMddHHmmsszzz")}||{messageType}|{messageControlID}|P|2.5|||AL|NE";

            string ack = $"MSA|AA|{messageControlID}";

            byte[] bytes1 = { 0x0b };
            string str1 = Encoding.ASCII.GetString(bytes1);

            byte[] bytes2 = { 0x1c };
            string str2 = Encoding.ASCII.GetString(bytes2);

            byte[] bytes3 = { 0x0d };
            string str3 = Encoding.ASCII.GetString(bytes3);

            string ackMessage = $"{str1}{msh}\r{ack}{str2}{str3}\r";

            //string ackMessage = $"{msh}\r\n{ack}\r\n";

            //string ackMessage = "MSH|^~\\&|||||20230509184119||" + messageType+"|" + messageControlID + "|P|2.5|||AL|NE\r\n";
            //ackMessage += "MSA|AA|" + messageControlID + "\r\n";

            return ackMessage;
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            serialInformation[sp.PortName].currentMessage = data;
            serialInformation[sp.PortName].message.Append(data);
            serialInformation[sp.PortName].isConnection = true;
            AddLog($"Received data from {sp.PortName}: {data}");
            if (serialInformation[sp.PortName].machine == SerialInfo.Machine.None)
            {
                if (serialInformation[sp.PortName].Contains(""))
                    serialInformation[sp.PortName].machine = SerialInfo.Machine.ISmartCare;
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
                        string strACK = Encoding.ASCII.GetString(ack);

                        AddChat(strACK);

                        await messageReceivedTcs.Task;

                        byte[] responseData = Encoding.ASCII.GetBytes(inputAck);
                        serialInformation[portName].port.Write(responseData, 0, responseData.Length);

                        AddLog("Server Response : " + inputAck);

                        messageReceivedTcs = new TaskCompletionSource<bool>();

                        //serialInformation[portName].ACK(SerialInfo.Machine.ISmartCare, ack);

                        //AddLog("sends an ACK message to the client.");
                        //buffer = string.Empty;
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


        void AddLog(string message)
        {
            Invoke((MethodInvoker)delegate
            {
                LogTextBox.AppendText(message + "\r\n");
                LogTextBox.ScrollToCaret();
            });
        }

        void AddChat(string message)
        {
            Invoke((MethodInvoker)delegate
            {
                InputBox.Text = message;
                inputAck = message;
            });
        }

        public static IPAddress GetLocalIPAddress()
        {
            // 호스트 이름을 가져옵니다
            string hostName = Dns.GetHostName();

            // 호스트 이름으로 IP 주소 배열을 가져옵니다
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // 주소 배열에서 IPv4 주소만 필터링합니다
            IPAddress ipv4Address = addresses.FirstOrDefault(
                address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            return ipv4Address;
        }
        public void Stop()
        {
            tcpListener.Stop();
            foreach (KeyValuePair<string, SerialInfo> item in serialInformation)
                item.Value.port.Close();
        }
        private void Connection_Click(object sender, EventArgs e)
        {

            if (currentPort.IsOpen)
                portList[PortNameBox.SelectedIndex].Close();
            else
            {
                try
                {
                    portList[PortNameBox.SelectedIndex].Open();
                }
                catch (Exception ex)
                {
                    // 시리얼 포트 열기 거부 예외 처리
                    Console.WriteLine("시리얼 포트 열기 거부: " + ex.Message);
                }
            }
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Invoke((MethodInvoker)delegate
                {
                    if (currentPort.IsOpen)
                    {
                        PortStateBox.Text = "열림";
                        Connection.Text = "연결끊기";
                    }
                    else
                    {
                        PortStateBox.Text = "닫침";
                        Connection.Text = "연결";
                    }

                    ClientsCountBox.Text = clientsCount.ToString();
                });
            });
        }

        private void PortNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPort = portList[PortNameBox.SelectedIndex];
        }
        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                inputAck = InputBox.Text;
                //LogTextBox.AppendText(InputBox.Text);
                InputBox.Text = string.Empty;

                messageReceivedTcs.SetResult(true);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            inputAck = InputBox.Text;
            //LogTextBox.AppendText(InputBox.Text);
            InputBox.Text = string.Empty;

            messageReceivedTcs.SetResult(true);
        }

        private void Agent_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private void LogTextBox_TextChanged(object sender, EventArgs e)
        {
            if (LogTextBox.Visible && LogTextBox.Enabled)
            {
                LogTextBox.SelectionStart = LogTextBox.Text.Length;
                LogTextBox.ScrollToCaret();
            }
        }
    }
}
