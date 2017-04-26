using System;

namespace Inchvi.IO.Command
{
    public class CommandResponse
    {
        public string Data { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? ReceiveTime { get; set; }
    }
}