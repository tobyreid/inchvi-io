using System;
using System.Diagnostics;
using System.Threading;
using Inchvi.IO.Channel;
using Inchvi.IO.Command;
using Inchvi.IO.Message;
using Inchvi.IO.Util;

namespace Inchvi.IO.Client
{
    public abstract class ChannelClient : IClient
    {
        private readonly object _sendLock = new object();
        private readonly IChannel _channel;
        private const int CommandTimeout = 5000;
        private const int CommandIntervalMs = 100;
        private DateTime _lastCommand;
        private IDeviceCommand _currentCommand;
        private readonly AutoResetEvent _commandInterval = new AutoResetEvent(true);
        private readonly AutoResetEvent _commandWait = new AutoResetEvent(false);
        public event Action<byte[]> EventReceived;
        protected ChannelClient(IChannel channel, IMessageAssembler messageAssembler)
        {
            _channel = channel;
            _channel.DataReceived += messageAssembler.OnDataReceived;
            messageAssembler.MessageAssembled += OnMessageAssembled;
        }

        protected abstract byte[] Acknowledge(byte[] bytes);
        protected abstract byte[] ParseReceive(byte[] bytes);
        protected abstract byte[] ParseSend(IDeviceCommand command);
        private void OnMessageAssembled(byte[] bytes)
        {
            var data = ParseReceive(bytes); //Parse incoming
            EventReceived?.Invoke(data); //Trigger EventReceived for all messages
            if (_currentCommand != null)
            {
                lock (_currentCommand)
                {
                    if (_currentCommand.ProcessData(data)) //Sucessfully processed the command for a command
                    {
                        if (_currentCommand.IsResponseExpected) //Only send the ACK for commands that return a response
                        {
                            var acknowledge = Acknowledge(bytes);
                            if (acknowledge != null)
                                _channel.Send(acknowledge);
                        }
                        _commandWait.Set();
                    }
                }

            }
            else
            {
                var acknowledge = Acknowledge(bytes);
                if (acknowledge != null)
                    _channel.Send(acknowledge);
            }
        }
        public CommandResponse Send(IDeviceCommand command, CancellationToken cancellationToken)
        {
            lock (_sendLock)
            {
                var nextCommand = (int)(Math.Max(0, (CommandIntervalMs - (DateTime.UtcNow - _lastCommand).TotalMilliseconds)));
                _currentCommand = command;
                _commandInterval.WaitOne(nextCommand);
                _currentCommand.SendTime = DateTime.UtcNow;
                _channel.Send(ParseSend(_currentCommand));
                _lastCommand = DateTime.UtcNow;
                var isSignalled = _commandWait.WaitOne(CommandTimeout, cancellationToken);
                if (isSignalled)
                {
                    _currentCommand.ReceiveTime = DateTime.UtcNow;
                    var commandResponse = _currentCommand.CommandResponse;
                    commandResponse.ReceiveTime = _currentCommand.ReceiveTime;
                    _currentCommand = null;
                    return commandResponse;
                }
                Trace.TraceError("TIMEOUT");
                _currentCommand = null;
                return null;
            }
        }
    }
}