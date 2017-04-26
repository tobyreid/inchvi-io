using System;

namespace Inchvi.IO.Channel
{
    /// <summary>
    /// Provides channel I/O 
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Raised when underlying serial port stream receives a block of data, not guaranteed to be the complete message or only a single message
        /// </summary>
        event Action<byte[]> DataReceived;
        /// <summary>
        /// Raised when and IOException occurs
        /// </summary>
        event Action<Exception> Error;
        /// <summary>
        /// Send byte array to connected channel
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);
    }
}