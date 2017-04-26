using System;
using System.Threading;
using Inchvi.IO.Command;

namespace Inchvi.IO.Client
{
    /// <summary>
    /// Provides synchronous command handling and events
    /// </summary>
    internal interface IClient
    {
        /// <summary>
        /// Raised when an solicited or unsolicited event is received
        /// </summary>
        event Action<byte[]> EventReceived;

        /// <summary>
        /// Sends a command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns a response or null</returns>
        CommandResponse Send(IDeviceCommand command, CancellationToken cancellationToken);
    }
}