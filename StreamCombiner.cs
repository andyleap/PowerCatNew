using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PowerCat
{
    public class StreamCombiner : Stream
    {
        private Stream In;
        private Stream Out;

        public StreamCombiner(Stream In, Stream Out)
        {
            this.In = In;
            this.Out = Out;
        }

        public override void Close()
        {
            base.Close();
            In.Close();
            Out.Close();
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
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return In.Read(buffer, offset, count);
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
            Out.Write(buffer, offset, count);
        }
    }
}
