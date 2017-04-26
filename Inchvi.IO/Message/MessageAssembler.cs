using System;
using System.Text;

namespace Inchvi.IO.Message
{
    public class MessageAssembler : IMessageAssembler
    {
        private readonly byte[] _delimiterBytes;
        public event Action<byte[]> MessageAssembled;
        byte[] _buffer;

        public MessageAssembler(char[] delimiterChars):this(Encoding.ASCII.GetBytes(delimiterChars))
        {
        }
        public MessageAssembler(byte[] delimiterBytes)
        {
            _delimiterBytes = delimiterBytes;
        }

        public void OnDataReceived(byte[] data)
        {
            var offset = 0;
            while (true)
            {
                var delimiterIndex = 0;
                foreach (var delimiterByte in _delimiterBytes) //Look for delimiters in the byte stream
                {
                    delimiterIndex = Array.IndexOf(data, delimiterByte, offset);
                    if(delimiterIndex != -1)
                        break;
                }
                if (delimiterIndex < offset)
                {
                    _buffer = ConcatArray(_buffer, data, offset, data.Length - offset);// not received a complete message
                    return;
                }
                ++delimiterIndex;
                var message = ConcatArray(_buffer, data, offset, delimiterIndex - offset);//concat everything in the buffer
                _buffer = null;
                offset = delimiterIndex;
                MessageAssembled?.Invoke(message); // raise an event for further processing
            }
        }

        static byte[] ConcatArray(byte[] head, byte[] tail, int tailOffset, int tailCount)
        {
            byte[] result;
            if (head == null)
            {
                result = new byte[tailCount];
                Array.Copy(tail, tailOffset, result, 0, tailCount);
            }
            else
            {
                result = new byte[head.Length + tailCount];
                head.CopyTo(result, 0);
                Array.Copy(tail, tailOffset, result, head.Length, tailCount);
            }

            return result;
        }
    }
}