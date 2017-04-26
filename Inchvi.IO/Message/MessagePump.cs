using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inchvi.IO.Message
{
    [Obsolete("Experimental")]
    public class MessagePump
    {
        public Action<byte[]> Consume;
        public MessagePump()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    // Consume consume the BlockingCollection
                    while (true)
                    {
                        var data = _blockingCollection.Take();
                        Consume?.Invoke(data);
                    }
                }
                catch (InvalidOperationException)
                {
                    // An InvalidOperationException means that Take() was called on a completed collection
                    Trace.WriteLine("That's All!");
                }
            });
        }
        private readonly BlockingCollection<byte[]> _blockingCollection = new BlockingCollection<byte[]>(); 
        public void Add(byte[] data)
        {
            if(_blockingCollection.Count > 1)
            Trace.WriteLine("BOOM" );
            _blockingCollection.Add(data);
        }

    }
}