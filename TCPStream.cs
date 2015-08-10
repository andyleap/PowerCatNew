using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace PowerCat
{
    public class TCPStream : Stream
    {
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        ManualResetEventSlim connected = new ManualResetEventSlim();

        public TCPStream(int Port)
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            listener.BeginAcceptTcpClient(Accept, null);
        }

        private void Accept(IAsyncResult ar)
        {
            if(listener == null)
            {
                return;
            }
            client = listener.EndAcceptTcpClient(ar);
            if(client == null)
            {
                listener.BeginAcceptTcpClient(Accept, null);
                return;
            }
            stream = client.GetStream();
            connected.Set();
        }

        public TCPStream(string Host, int Port)
        {
            client = new TcpClient();
            client.BeginConnect(Host, Port, Connect, null);
        }

        private void Connect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            stream = client.GetStream();
            connected.Set();
        }

        public override void Close()
        {
            connected.Reset();
            base.Close();
            if(stream != null)
            {
                stream.Close();
                stream = null;
            }
            if(client != null && client.Connected)
            {
                client.Close();
                client = null;
            }
            if(listener != null)
            {
                listener.Stop();
                listener = null;
            }
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
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            if(stream != null)
            {
                stream.Flush();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if(stream != null)
            {
                return stream.Read(buffer, offset, count);
            }
            else
            {
                connected.Wait();
                return stream.Read(buffer, offset, count);
            }
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
            if (stream != null)
            {
                stream.Write(buffer, offset, count);
            }
            else
            {
                connected.Wait();
                stream.Write(buffer, offset, count);
            }
        }
    }
}
