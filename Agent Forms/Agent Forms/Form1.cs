using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.IO;

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
        bool isAutoAckSend;
        string inputAck;

        public Agent()
        {
            InitializeComponent();

            for (int i = 0; i < SerialPort.GetPortNames().Length; i++)
            {
                string name = SerialPort.GetPortNames()[i];
                PortNameBox.Items.Add(name);
                portList.Add(new SerialPort(name));
                portList[i].DataReceived += SerialPortDataReceived;
                portList[i].Open();
                serialInformation.Add(name, new SerialInfo(portList[i]));
            }

            PortNameBox.SelectedIndex = 0;
            currentPort = portList[0];

            for (int i = 1200; i <= 9600; i *=2)
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

            isAutoAckSend = true;
            AutoAckSendCheckBox.Checked = isAutoAckSend;
            InputBox.Enabled = false;
            SendButton.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                    if (isAutoAckSend)
                    {
                        if (receivedData.Contains("VA20DBITG1495"))
                        {
                            AddLog("VCheck Only Reply");

                            string mshGuid = Guid.NewGuid().ToString();

                            string msh = $"MSH|^~\\&amp;|Virtual HL7 Server^{mshGuid}^GUID|Instr RnD Dept|||{DateTimeOffset.Now.ToString("yyyyMMddHHmmsszzz")}|ACK^R01^ACK|{random.Next(1, 999999)}|P|2.6";

                            string ack = $"MSA|CA|{Guid.NewGuid()}";

                            string responseMessage = $"{msh}\n\r{ack}\n\r";

                            AddLog(responseMessage);

                            byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);

                            await stream.WriteAsync(responseData, 0, responseData.Length);
                        }
                        else
                        {
                            AddLog("sends an ACK message to the client.");
                            await stream.WriteAsync(ack, 0, ack.Length);
                        }
                    }
                    else
                    {
                        await messageReceivedTcs.Task;

                        byte[] responseData = Encoding.ASCII.GetBytes(inputAck);
                        await stream.WriteAsync(responseData, 0, responseData.Length);

                        messageReceivedTcs.SetResult(false);
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
                    if (isAutoAckSend)
                    {
                        if (!string.IsNullOrEmpty(buffer))
                        {
                            serialInformation[portName].ACK(SerialInfo.Machine.ISmartCare, ack);

                            AddLog("sends an ACK message to the client.");
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
                    else
                    {
                        await messageReceivedTcs.Task;

                        byte[] responseData = Encoding.ASCII.GetBytes(inputAck);
                        serialInformation[portName].port.Write(responseData, 0, responseData.Length);

                        messageReceivedTcs.SetResult(false);
                    }
                }
                await Task.Delay(100);
            }
        }

        private void HandleClient(TcpClient tcpClient)
        {
            try
            {
                // 클라이언트 IP 주소 가져오기
                string clientIPAddress = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();

                // 로그에 클라이언트 접속 메시지 출력
                string message = $"클라이언트 접속됨 - IP: {clientIPAddress}";
                AddLog(message);

                // 데이터 받기
                byte[] buffer = new byte[1024];
                int bytesReceived = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                // 데이터 출력
                AddLog($"받은 데이터: {data}");

                // 데이터 보내기
                byte[] bytesToSend = Encoding.UTF8.GetBytes("서버에서 보낸 메시지입니다.");
                tcpClient.GetStream().Write(bytesToSend, 0, bytesToSend.Length);

                // 클라이언트와 연결 종료
                tcpClient.Close();

                // 로그에 클라이언트 연결 종료 메시지 출력
                AddLog("클라이언트와 연결 종료됨");
            }
            catch (Exception ex)
            {
                AddLog($"클라이언트 처리 실패 - {ex.Message}");
            }
        }

        private void AddLog(string message)
        {
            Invoke((MethodInvoker)delegate
            {
                LogBox.Items.Add(message);
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

            if(currentPort.IsOpen)
                portList[PortNameBox.SelectedIndex].Close();
            else
                portList[PortNameBox.SelectedIndex].Open();
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
        private void AutoAckSendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoAckSendCheckBox.Checked)
            {
                //AddLog("Automatic ack sending");
                isAutoAckSend = true;
                InputBox.Enabled = false;
                SendButton.Enabled = false;
            }
            else
            {
                //AddLog("Send manual ACK");
                isAutoAckSend = false;
                InputBox.Enabled = true;
                SendButton.Enabled = true;
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                inputAck = InputBox.Text;
                LogBox.Items.Add(InputBox.Text);
                InputBox.Text = string.Empty;

                messageReceivedTcs.SetResult(true);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            inputAck = InputBox.Text;
            LogBox.Items.Add(InputBox.Text);
            InputBox.Text = string.Empty;

            messageReceivedTcs.SetResult(true);
        }

        private void Agent_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
    }
}
