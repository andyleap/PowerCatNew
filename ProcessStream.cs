using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PowerCat
{
    public class ProcessStream : Stream
    {
        private Process process;

        public ProcessStream(string filename, string arguments)
        {
            process = new Process();
            process.StartInfo = new ProcessStartInfo(filename, arguments);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
        }

        public void Start()
        {
            process.Start();
        }

        public void Wait()
        {
            process.WaitForExit();
        }

        public override void Close()
        {
            base.Close();
            if (!process.HasExited)
            {
                process.Kill();
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
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int n = process.StandardOutput.BaseStream.Read(buffer, offset, count);
            return n;
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
            process.StandardInput.BaseStream.Write(buffer, offset, count);
            process.StandardInput.BaseStream.Flush();
        }
    }
}
