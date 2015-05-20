using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Websocket.Samples
{
    class Program
    {
        private static WebChatServer mChatServer;

        private static CpuUsageServer mCpuUsageServer;

        private static HelloServer mHelloServer;

        private static DataServer mDataServer;

        static void Main(string[] args)
        {
            mChatServer = new WebChatServer();
            mChatServer.Open(9123);
            Console.WriteLine("webchat server start:{0}@{1}", mChatServer.Server.Host, mChatServer.Server.Port);

            mCpuUsageServer = new CpuUsageServer();
            mCpuUsageServer.Open(9124);
            Console.WriteLine("cpu usage server start:{0}@{1}", mCpuUsageServer.Server.Host, mCpuUsageServer.Server.Port);

            mHelloServer = new HelloServer();
            mHelloServer.Open(9125);
            Console.WriteLine("hello server start:{0}@{1}", mHelloServer.Server.Host, mHelloServer.Server.Port);

            mDataServer = new DataServer();
            mDataServer.Open(9126);
            Console.WriteLine("data server start:{0}@{1}", mDataServer.Server.Host, mDataServer.Server.Port);
            
            System.Threading.Thread.Sleep(-1);
        }
    }
}
