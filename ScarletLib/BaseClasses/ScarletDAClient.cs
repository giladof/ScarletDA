using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScarletLib.BaseClasses
{
    public class ScarletDAClient
    {
 public  bool startme = false;
        public class StreamString
        {
            private Stream ioStream;
            private UnicodeEncoding streamEncoding;
           
            public StreamString(Stream ioStream)
            {
                this.ioStream = ioStream;
                streamEncoding = new UnicodeEncoding();
            }

            public string ReadString()
            {
                int len = 0;

                len = ioStream.ReadByte() * 256;
                len += ioStream.ReadByte();
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);

                return streamEncoding.GetString(inBuffer);
            }

            public int WriteString(string outString)
            {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;
                if (len > UInt16.MaxValue)
                {
                    len = (int)UInt16.MaxValue;
                }
                ioStream.WriteByte((byte)(len / 256));
                ioStream.WriteByte((byte)(len & 255));
                ioStream.Write(outBuffer, 0, len);
                ioStream.Flush();

                return outBuffer.Length + 2;
            }
        }
        public void ClientRun(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("ScarletDAPipeInit", PipeDirection.InOut, _numThreads);
           int threadId = Thread.CurrentThread.ManagedThreadId;
            ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAService Before wait for connection!", AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            //   Console.WriteLine("Client connected on thread[{0}].", threadId);
            try
            {
                StreamString ss = new StreamString(pipeServer);
                string fromClient = ss.ReadString();
                if (fromClient.StartsWith("Thread"))
                   {
                    //   ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAClient sent:" + fromClient, AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");

                       ScarletDAServer.ClientThread= uint.Parse( fromClient.Split(':')[1]);
                 //   ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAServer.ClientThread:" + ScarletDAServer.ClientThread, AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");
                    pipeServer.Close();
                    return;
                   }
              
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
               // ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDA Client Error: !" + e.Message, AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt");
            }
            pipeServer.Close();
        }

        public void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("ScarletDAPipe", PipeDirection.InOut, _numThreads);
           
            int threadId = Thread.CurrentThread.ManagedThreadId;
         //   ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAClient before waiting for connection", AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");

            // Wait for a client to connect
            pipeServer.WaitForConnection();
            
            //   Console.WriteLine("Client connected on thread[{0}].", threadId);
            try
            {
                StreamString ss = new StreamString(pipeServer);
                string fromClient = ss.ReadString();
                if (fromClient == "Start")
                {
          //          ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAServer sent:" + fromClient, AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");

                    startme = true;
                    return;
                }
            
            //    ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDAServer sent:" + fromClient, AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
                ScarletLib.BaseClasses.Program ClientProgram = new Program("ClientProgram", fromClient, null);
                ClientProgram.ProgramChanged += ClientProgram_ProgramChanged;
                ClientProgram.Runme();
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
              //  ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDA Client Error: !" + e.Message, AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
            }
            pipeServer.Close();
        }

        private void ClientProgram_ProgramChanged(object sender, EventArgs e)
        {
            ScarletLib.BaseClasses.ScarletLogger.LogMessage(String.Format("Raised event that program has changed: current value is {0}", (sender as ScarletLib.BaseClasses.Program).ProgramState), AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
            
        }

        int _numThreads=1;
        public  bool StartClient(int numThreads)
        {
            try
            {
                int i;
                _numThreads = numThreads;
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDA Client Starting!", AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
                Thread[] servers = new Thread[numThreads];
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDA Client Number of threads:"+numThreads, AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");

                for (i = 0; i < numThreads; i++)
                {
                    servers[i] = new Thread(ServerThread);
                    servers[i].Start();
                    ScarletLib.BaseClasses.ScarletLogger.LogMessage(String.Format("ScarletDA Client thread number {0} Started!",i), AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");

                }

            }
            catch (Exception e)
            {
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ScarletDA Client Error: !"+e.Message, AppDomain.CurrentDomain.BaseDirectory + "ClientLog.txt");
                return false;
            }
                return true;
          
        }
    }
}
