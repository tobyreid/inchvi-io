using System;
using Inchvi.IO.Command;

namespace Inchvi.Tcpx.Command
{
    public abstract class TcpxBaseCommand : IDeviceCommand
    {
        protected TcpxBaseCommand()
        {
            CommandResponse = new CommandResponse();
        }
        public abstract string RequestCommand { get; }

        public abstract string CommandStartChar { get; }
        public abstract string CommandEndChar { get; }

        public virtual string CommandName
        {
            get { return GetType().Name; }
        }
        public virtual bool IsResponseExpected
        {
            get { return true; }
        }

        public bool ProcessData(byte[] data)
        {
            if (data == null)
                return false;
            return TryParseCommandResponse(data);
        }

        public virtual string Ack
        {
            get { return string.Empty; }
        }
        public DateTime SendTime { get; set; }
        public DateTime ReceiveTime { get; set; }
        public CommandResponse CommandResponse { get; protected set; }
        protected abstract bool TryParseCommandResponse(byte[] data);
    }
}