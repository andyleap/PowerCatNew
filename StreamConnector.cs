using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace PowerCat
{
    public class StreamConnector 
    {
        private Stream A;
        private Stream B;

        private int BufferSize = 1024;

        private byte[] BufferA;
        private byte[] BufferB;

        private ManualResetEvent Finished = new ManualResetEvent(false);

        public StreamConnector(Stream A, Stream B)
        {
            this.A = A;
            this.B = B;
            BufferA = new byte[BufferSize];
            BufferB = new byte[BufferSize];
        }

        public void Connect()
        {
            A.BeginRead(BufferA, 0, BufferSize, FinishRead, new State(A, B, BufferA));
            B.BeginRead(BufferB, 0, BufferSize, FinishRead, new State(B, A, BufferB));
        }

        public void Wait()
        {
            Finished.WaitOne();
        }

        private class State
        {
            internal Stream Read;
            internal Stream Write;
            internal byte[] Buffer;
            internal State(Stream Read, Stream Write, byte[] Buffer)
            {
                this.Read = Read;
                this.Write = Write;
                this.Buffer = Buffer;
            }
        }

        private void FinishRead(IAsyncResult ar)
        {
            State state = ar.AsyncState as State;
            int read = 0;
            try
            {
                read = state.Read.EndRead(ar);
            }
            catch
            {
            }
            if (read == 0)
            {
                Finished.Set();
                A.Close();
                B.Close();
                return;
            }
            state.Write.Write(state.Buffer, 0, read);
            state.Read.BeginRead(state.Buffer, 0, BufferSize, FinishRead, state);
        }
    }
}
