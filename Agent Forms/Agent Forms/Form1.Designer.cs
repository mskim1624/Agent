using System.Drawing;
using System.Windows.Forms;

namespace Agent_Forms
{
    partial class Agent
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.IPBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PortLabel = new System.Windows.Forms.Label();
            this.TCPLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AutoAckSendCheckBox = new System.Windows.Forms.CheckBox();
            this.SerialLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Connection = new System.Windows.Forms.Button();
            this.PortStateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DataBitsLabel = new System.Windows.Forms.Label();
            this.ParityLabel = new System.Windows.Forms.Label();
            this.BaudRateLabel = new System.Windows.Forms.Label();
            this.SerialPortLabel = new System.Windows.Forms.Label();
            this.SerialStateLabel = new System.Windows.Forms.Label();
            this.Box4 = new System.Windows.Forms.ComboBox();
            this.Box3 = new System.Windows.Forms.ComboBox();
            this.Box2 = new System.Windows.Forms.ComboBox();
            this.Box1 = new System.Windows.Forms.ComboBox();
            this.PortNameBox = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.LogBox = new System.Windows.Forms.ListBox();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.SendButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ClientsCountBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // IPBox
            // 
            this.IPBox.AcceptsTab = true;
            this.IPBox.Enabled = false;
            this.IPBox.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.IPBox.Location = new System.Drawing.Point(81, 15);
            this.IPBox.Name = "IPBox";
            this.IPBox.ReadOnly = true;
            this.IPBox.Size = new System.Drawing.Size(145, 27);
            this.IPBox.TabIndex = 0;
            this.IPBox.Text = "123.123.123.123";
            this.IPBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PortBox
            // 
            this.PortBox.AcceptsTab = true;
            this.PortBox.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.PortBox.Location = new System.Drawing.Point(109, 58);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(78, 27);
            this.PortBox.TabIndex = 1;
            this.PortBox.Text = "7777";
            this.PortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.IPLabel.Location = new System.Drawing.Point(35, 18);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(23, 17);
            this.IPLabel.TabIndex = 2;
            this.IPLabel.Text = "IP";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.ClientsCountBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.PortLabel);
            this.panel1.Controls.Add(this.IPLabel);
            this.panel1.Controls.Add(this.PortBox);
            this.panel1.Controls.Add(this.IPBox);
            this.panel1.Location = new System.Drawing.Point(12, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 151);
            this.panel1.TabIndex = 3;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.PortLabel.Location = new System.Drawing.Point(35, 61);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(42, 17);
            this.PortLabel.TabIndex = 3;
            this.PortLabel.Text = "Port";
            // 
            // TCPLabel
            // 
            this.TCPLabel.AutoSize = true;
            this.TCPLabel.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
            this.TCPLabel.Location = new System.Drawing.Point(27, 15);
            this.TCPLabel.Name = "TCPLabel";
            this.TCPLabel.Size = new System.Drawing.Size(45, 19);
            this.TCPLabel.TabIndex = 4;
            this.TCPLabel.Text = "TCP";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.AutoAckSendCheckBox);
            this.panel2.Controls.Add(this.SerialLabel);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.TCPLabel);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(297, 559);
            this.panel2.TabIndex = 5;
            // 
            // AutoAckSendCheckBox
            // 
            this.AutoAckSendCheckBox.AutoSize = true;
            this.AutoAckSendCheckBox.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AutoAckSendCheckBox.Location = new System.Drawing.Point(123, 516);
            this.AutoAckSendCheckBox.Name = "AutoAckSendCheckBox";
            this.AutoAckSendCheckBox.Size = new System.Drawing.Size(155, 21);
            this.AutoAckSendCheckBox.TabIndex = 9;
            this.AutoAckSendCheckBox.Text = "Auto ACK Send";
            this.AutoAckSendCheckBox.UseVisualStyleBackColor = true;
            this.AutoAckSendCheckBox.CheckedChanged += new System.EventHandler(this.AutoAckSendCheckBox_CheckedChanged);
            // 
            // SerialLabel
            // 
            this.SerialLabel.AutoSize = true;
            this.SerialLabel.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
            this.SerialLabel.Location = new System.Drawing.Point(26, 204);
            this.SerialLabel.Name = "SerialLabel";
            this.SerialLabel.Size = new System.Drawing.Size(58, 19);
            this.SerialLabel.TabIndex = 5;
            this.SerialLabel.Text = "Serial";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.Connection);
            this.panel3.Controls.Add(this.PortStateBox);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.DataBitsLabel);
            this.panel3.Controls.Add(this.ParityLabel);
            this.panel3.Controls.Add(this.BaudRateLabel);
            this.panel3.Controls.Add(this.SerialPortLabel);
            this.panel3.Controls.Add(this.SerialStateLabel);
            this.panel3.Controls.Add(this.Box4);
            this.panel3.Controls.Add(this.Box3);
            this.panel3.Controls.Add(this.Box2);
            this.panel3.Controls.Add(this.Box1);
            this.panel3.Controls.Add(this.PortNameBox);
            this.panel3.Location = new System.Drawing.Point(13, 226);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 266);
            this.panel3.TabIndex = 6;
            // 
            // Connection
            // 
            this.Connection.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
            this.Connection.Location = new System.Drawing.Point(42, 219);
            this.Connection.Name = "Connection";
            this.Connection.Size = new System.Drawing.Size(183, 32);
            this.Connection.TabIndex = 12;
            this.Connection.Text = "연결";
            this.Connection.UseVisualStyleBackColor = true;
            this.Connection.Click += new System.EventHandler(this.Connection_Click);
            // 
            // PortStateBox
            // 
            this.PortStateBox.AcceptsTab = true;
            this.PortStateBox.Enabled = false;
            this.PortStateBox.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.PortStateBox.Location = new System.Drawing.Point(150, 175);
            this.PortStateBox.Name = "PortStateBox";
            this.PortStateBox.ReadOnly = true;
            this.PortStateBox.Size = new System.Drawing.Size(102, 27);
            this.PortStateBox.TabIndex = 4;
            this.PortStateBox.Text = "Close";
            this.PortStateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(38, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Stop Bits";
            // 
            // DataBitsLabel
            // 
            this.DataBitsLabel.AutoSize = true;
            this.DataBitsLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.DataBitsLabel.Location = new System.Drawing.Point(39, 107);
            this.DataBitsLabel.Name = "DataBitsLabel";
            this.DataBitsLabel.Size = new System.Drawing.Size(82, 17);
            this.DataBitsLabel.TabIndex = 10;
            this.DataBitsLabel.Text = "Data Bits";
            // 
            // ParityLabel
            // 
            this.ParityLabel.AutoSize = true;
            this.ParityLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ParityLabel.Location = new System.Drawing.Point(67, 78);
            this.ParityLabel.Name = "ParityLabel";
            this.ParityLabel.Size = new System.Drawing.Size(54, 17);
            this.ParityLabel.TabIndex = 9;
            this.ParityLabel.Text = "Parity";
            // 
            // BaudRateLabel
            // 
            this.BaudRateLabel.AutoSize = true;
            this.BaudRateLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.BaudRateLabel.Location = new System.Drawing.Point(29, 49);
            this.BaudRateLabel.Name = "BaudRateLabel";
            this.BaudRateLabel.Size = new System.Drawing.Size(92, 17);
            this.BaudRateLabel.TabIndex = 8;
            this.BaudRateLabel.Text = "Baud Rate";
            // 
            // SerialPortLabel
            // 
            this.SerialPortLabel.AutoSize = true;
            this.SerialPortLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.SerialPortLabel.Location = new System.Drawing.Point(34, 20);
            this.SerialPortLabel.Name = "SerialPortLabel";
            this.SerialPortLabel.Size = new System.Drawing.Size(87, 17);
            this.SerialPortLabel.TabIndex = 7;
            this.SerialPortLabel.Text = "COM Port";
            // 
            // SerialStateLabel
            // 
            this.SerialStateLabel.AutoSize = true;
            this.SerialStateLabel.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.SerialStateLabel.Location = new System.Drawing.Point(37, 178);
            this.SerialStateLabel.Name = "SerialStateLabel";
            this.SerialStateLabel.Size = new System.Drawing.Size(84, 17);
            this.SerialStateLabel.TabIndex = 6;
            this.SerialStateLabel.Text = "Port 상태";
            // 
            // Box4
            // 
            this.Box4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Box4.FormattingEnabled = true;
            this.Box4.Location = new System.Drawing.Point(150, 135);
            this.Box4.Name = "Box4";
            this.Box4.Size = new System.Drawing.Size(102, 23);
            this.Box4.TabIndex = 4;
            // 
            // Box3
            // 
            this.Box3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Box3.FormattingEnabled = true;
            this.Box3.Location = new System.Drawing.Point(150, 106);
            this.Box3.Name = "Box3";
            this.Box3.Size = new System.Drawing.Size(102, 23);
            this.Box3.TabIndex = 3;
            // 
            // Box2
            // 
            this.Box2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Box2.FormattingEnabled = true;
            this.Box2.Location = new System.Drawing.Point(150, 77);
            this.Box2.Name = "Box2";
            this.Box2.Size = new System.Drawing.Size(102, 23);
            this.Box2.TabIndex = 2;
            // 
            // Box1
            // 
            this.Box1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Box1.FormattingEnabled = true;
            this.Box1.Location = new System.Drawing.Point(150, 48);
            this.Box1.Name = "Box1";
            this.Box1.Size = new System.Drawing.Size(102, 23);
            this.Box1.TabIndex = 1;
            // 
            // PortNameBox
            // 
            this.PortNameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortNameBox.FormattingEnabled = true;
            this.PortNameBox.Location = new System.Drawing.Point(150, 19);
            this.PortNameBox.Name = "PortNameBox";
            this.PortNameBox.Size = new System.Drawing.Size(102, 23);
            this.PortNameBox.TabIndex = 0;
            this.PortNameBox.SelectedIndexChanged += new System.EventHandler(this.PortNameBox_SelectedIndexChanged);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // LogBox
            // 
            this.LogBox.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LogBox.FormattingEnabled = true;
            this.LogBox.ItemHeight = 23;
            this.LogBox.Location = new System.Drawing.Point(15, 11);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(864, 464);
            this.LogBox.TabIndex = 6;
            // 
            // InputBox
            // 
            this.InputBox.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.InputBox.Location = new System.Drawing.Point(15, 503);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(782, 34);
            this.InputBox.TabIndex = 7;
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_KeyDown);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.SendButton);
            this.panel4.Controls.Add(this.InputBox);
            this.panel4.Controls.Add(this.LogBox);
            this.panel4.Location = new System.Drawing.Point(332, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(896, 559);
            this.panel4.TabIndex = 8;
            // 
            // SendButton
            // 
            this.SendButton.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SendButton.Location = new System.Drawing.Point(803, 503);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(76, 28);
            this.SendButton.TabIndex = 8;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(30, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Clients";
            // 
            // ClientsCountBox
            // 
            this.ClientsCountBox.AcceptsTab = true;
            this.ClientsCountBox.Enabled = false;
            this.ClientsCountBox.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ClientsCountBox.Location = new System.Drawing.Point(109, 101);
            this.ClientsCountBox.Name = "ClientsCountBox";
            this.ClientsCountBox.ReadOnly = true;
            this.ClientsCountBox.Size = new System.Drawing.Size(78, 27);
            this.ClientsCountBox.TabIndex = 5;
            this.ClientsCountBox.Text = "0";
            this.ClientsCountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Agent
            // 
            this.ClientSize = new System.Drawing.Size(1240, 598);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Name = "Agent";
            this.Text = "Agent Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox IPBox;
        private System.Windows.Forms.TextBox PortBox;
        private Label IPLabel;
        private Panel panel1;
        private Label PortLabel;
        private Label TCPLabel;
        private Panel panel2;
        private Label SerialLabel;
        private Panel panel3;
        private Label SerialStateLabel;
        private ComboBox Box4;
        private ComboBox Box3;
        private ComboBox Box2;
        private ComboBox Box1;
        private ComboBox PortNameBox;
        private Label SerialPortLabel;
        private TextBox PortStateBox;
        private Label label1;
        private Label DataBitsLabel;
        private Label ParityLabel;
        private Label BaudRateLabel;
        private Button Connection;
        private Timer timer;
        private ListBox LogBox;
        private TextBox InputBox;
        private Panel panel4;
        private CheckBox AutoAckSendCheckBox;
        private Button SendButton;
        private TextBox ClientsCountBox;
        private Label label2;
    }
}

