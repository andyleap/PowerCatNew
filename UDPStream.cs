using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace PowerCat
{
    class UDPStream : Stream
    {
        private UdpClient client;
        private IPEndPoint endpoint;

        public UDPStream(string Host, int Port)
        {
            endpoint = new IPEndPoint(IPAddress.Any, Port);
            client = new UdpClient(Host, Port);
        }

        public UDPStream(int Port)
        {
            endpoint = new IPEndPoint(IPAddress.Any, Port);
            client = new UdpClient(Port);
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                return 0;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] data = client.Receive(ref endpoint);
            data.CopyTo(buffer, offset);
            return data.Length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (endpoint.Address != IPAddress.Any)
            {
                

                //client.Send();
            }
        }
    }
}
