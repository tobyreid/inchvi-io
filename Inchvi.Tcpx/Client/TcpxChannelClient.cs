using System;
using System.Text;
using Inchvi.IO.Channel;
using Inchvi.IO.Client;
using Inchvi.IO.Command;
using Inchvi.IO.Message;

namespace Inchvi.Tcpx.Client
{
    class TcpxChannelClient : ChannelClient
    {
        public TcpxChannelClient(IChannel channel, IMessageAssembler messageAssembler) : base(channel, messageAssembler)
        {
        }

        protected override byte[] Acknowledge(byte[] bytes)
        {
            return Encoding.ASCII.GetBytes("\u0006");
        }

        protected override byte[] ParseReceive(byte[] bytes)
        {
            return bytes;
        }

        protected override byte[] ParseSend(IDeviceCommand command)
        {
            var request = command.CommandStartChar + command.RequestCommand + command.CommandEndChar  +
                          CalculateLrc(command.RequestCommand + command.CommandEndChar);
            var data = Encoding.ASCII.GetBytes(request);
            return data;
        }
        private static char CalculateLrc(string toEncode)
        {
            var bytes = Encoding.ASCII.GetBytes(toEncode);
            byte lrc = 0;
            foreach (var b in bytes)
                lrc = (byte)(lrc ^ b);
            var c = Convert.ToChar(lrc);
            return c;
        }
    }
}