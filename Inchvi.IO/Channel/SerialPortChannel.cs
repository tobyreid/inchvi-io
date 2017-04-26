using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;

namespace Inchvi.IO.Channel
{
    public class SerialPortChannel : IChannel
    {
        private readonly string _portName;
        private readonly SerialPort _port;
        public event Action<byte[]> DataReceived;
        public event Action<Exception> Error;
        const int BlockLimit = 4096;

        public SerialPortChannel(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _portName = portName;
            _port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _port.Open();
            var buffer = new byte[BlockLimit];
            Action read = null;
            read = () => _port.BaseStream.BeginRead(buffer, 0, buffer.Length, ar =>
            {
                try
                {
                    int actualLength = _port.BaseStream.EndRead(ar);
                    byte[] received = new byte[actualLength];
                    Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                    Trace.WriteLine($"{_portName} RECV: " + _port.Encoding.GetString(received));
                    DataReceived?.Invoke(received);
                }
                catch (IOException exc)
                {
                    Error?.Invoke(exc);
                }
                read();
            }, null);
            read();
        }

        public void Send(byte[] data)
        {
            _port.Write(data, 0, data.Length);
            Trace.WriteLine($"{_portName} SEND: " + _port.Encoding.GetString(data));
        }
    }
}
