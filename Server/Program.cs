using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;

namespace Server { 
    class Program {
        static void Main(string[] args) {
            BaseMessage.Init();
            BaseRoom lobby = new BaseRoom();
            var sockServer = new TcpListener(System.Net.IPAddress.Parse("0.0.0.0"), 16384);
            sockServer.Start();            
            while (true) {
                Server.BaseConnection.New().Init( sockServer.AcceptTcpClient(), lobby );
            }
        }
    }
}
