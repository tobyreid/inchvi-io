using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using Inchvi.IO.Channel;
using Inchvi.IO.Message;
using Inchvi.Tcpx.Client;
using Inchvi.Tcpx.Command;

namespace Inchvi.Tcpx
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
            
        }

        private void Run()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            var channelClient = new TcpxChannelClient(new SerialPortChannel("COM6", 9600, Parity.None, 8, StopBits.One),
                    new MessageAssembler(new[] {'\u0006', '\u0015', '\u0003'}));

            var i = 0;
            while (true)
            {
                var command = new DisplayHandlingCommand(new Ipp320DisplayValue(i.ToString(), '|'));
                channelClient.Send(command, CancellationToken.None);
                i++;
            }
            Console.ReadKey();
        }

    }
}