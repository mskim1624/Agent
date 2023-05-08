using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent_Forms
{
    public class SerialInfo
    {
        public enum Machine
        {
            ISmartCare,
            None
        }

        public SerialPort port;
        public string currentMessage;
        public StringBuilder message;
        public Machine machine;
        public bool isConnection;

        public SerialInfo(SerialPort port)
        {
            this.port = port;
            message = new StringBuilder();
            Machine machine = Machine.None;
        }

        public void ACK(Machine machine, byte[] ack)
        {
            if (machine == Machine.ISmartCare)
            {
                port.Write(ack, 0, ack.Length);
                isConnection = false;
            }
        }

        public bool Contains(string machine)
        {
            if (message.ToString().Contains(machine))
                return true;

            return false;
        }

    }
}
