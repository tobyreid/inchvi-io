using System;

namespace Inchvi.IO.Message
{
    /// <summary>
    /// Buffers data received by a delimiter set and returns individual complete messages
    /// </summary>
    public interface IMessageAssembler
    {
        /// <summary>
        /// Raised when a complete message has been received
        /// </summary>
        event Action<byte[]> MessageAssembled;
        /// <summary>
        /// Enumerates the buffer looking for complete messages
        /// </summary>
        /// <param name="data"></param>
        void OnDataReceived(byte[] data);
    }
}