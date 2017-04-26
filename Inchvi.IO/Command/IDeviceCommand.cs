using System;

namespace Inchvi.IO.Command
{
    public interface IDeviceCommand
    {
        string CommandName { get; }
        string RequestCommand { get; }
        bool ProcessData(byte[] data);
        string Ack { get; }
        DateTime SendTime { get; set; }
        DateTime ReceiveTime { get; set; }
        bool IsResponseExpected { get; }
        CommandResponse CommandResponse { get; }
        string CommandStartChar { get; }
        string CommandEndChar { get; }
    }
}