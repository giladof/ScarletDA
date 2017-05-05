using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ScarletDAService
{
    class NamepipeListener
    {
        // Defines the data protocol for reading and writing strings on our stream
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
        static int numThreads;
        static string location;
        public static bool StartServer()
        {
             location = AppDomain.CurrentDomain.BaseDirectory + "ServerLog.log";
            int i;
            numThreads = Properties.Settings.Default.NumOfThreads;
            ScarletLib.BaseClasses.ScarletLogger.LogMessage(String.Format("Num of thread is: {0}.", numThreads), location);

            Thread[] servers = new Thread[numThreads];
            for (i = 0; i < numThreads; i++)
            {
                servers[i] = new Thread(ServerThread);
                ScarletLib.BaseClasses.ScarletLogger.LogMessage(String.Format("Starting thread number: {0}.", i), location);

                servers[i].Start();
                ScarletLib.BaseClasses.ScarletLogger.LogMessage(String.Format("Thread number: {0} started!.", i), location);

            }
            
            
            ScarletLib.BaseClasses.ScarletLogger.LogMessage("Done Starting Threads!.", location);
            
        
            return true;
        }

        private static void ServerThread(object data)
        {
            
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("ScarletDAPipe", PipeDirection.InOut, numThreads);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();
            string fromClient;
            try
            {
                StreamString ss = new StreamString(pipeServer);
                fromClient= ss.ReadString();
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("From Client:" + fromClient, location);
                ScarletLib.BaseClasses.Program ClientProgram = new ScarletLib.BaseClasses.Program("Client Initiated program", fromClient, "");
                ClientProgram.ProgramChanged += ClientProgram_ProgramChanged;
                ClientProgram.Runme();
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                ScarletLib.BaseClasses.ScarletLogger.LogMessage("ERROR: "+ e.Message,location);
            }
            pipeServer.Close();
        }

        private static void ClientProgram_ProgramChanged(object sender, EventArgs e)
        {
            ScarletLib.BaseClasses.ScarletLogger.LogMessage(string.Format("Raised event that program has changed: current value is {0}", (sender as ScarletLib.BaseClasses.Program).ProgramState), location);
        }
    }

}
