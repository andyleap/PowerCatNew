using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Management.Automation;

namespace PowerCat
{
    [Cmdlet(VerbsLifecycle.Invoke, "Powercat")]
    public class InvokePowercatCmdlet : Cmdlet
    {
        Connection[] conns = new Connection[] { new Connection(), new Connection() };
        int conn = -1;

        [Parameter]
        public string Client { set
            {
                conn++;
                conns[conn].Type = "TCP";
                conns[conn].Client = true;
                conns[conn].IP = value;
            }
        }

        [Parameter]
        public SwitchParameter Listener
        {
            set
            {
                if (value != false)
                {
                    conn++;
                    conns[conn].Type = "TCP";
                    conns[conn].Client = false;
                }
            }
        }

        [Parameter]
        public int Port
        {
            set
            {
                conns[conn].Port = value;
            }
        }

        [Parameter]
        public string Exec
        {
            set
            {
                conn++;
                conns[conn].Type = "EXEC";
                conns[conn].Executable = value;
            }
        }

        void Run()
        {
            Stream a = StreamBuilder(conns[0]);
            Stream b = StreamBuilder(conns[1]);
            StreamConnector connector = new StreamConnector(a, b);
            connector.Connect();
            connector.Wait();
        }

        static Stream StreamBuilder(Connection conn)
        {
            switch(conn.Type)
            {
                case "TCP":
                    if (conn.Client)
                    {
                        return new TCPStream(conn.IP, conn.Port);
                    }
                    else
                    {
                        return new TCPStream(conn.Port);
                    }
                case "EXEC":
                    var stream = new ProcessStream(conn.Executable, "");
                    stream.Start();
                    return stream;
                default:
                    return new StreamCombiner(Console.OpenStandardInput(), Console.OpenStandardOutput());
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            Run();
        }

        class Connection
        {
            public bool Client = false;
            public string IP = "";
            public int Port = 0;
            public string Type = "STD";
            public string Executable = "";

            public override string ToString()
            {
                return Type + " " + Client + " " + IP + " " + Port + " " + Executable;
            }
        }
    }
}
