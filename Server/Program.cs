using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;
using System.Threading;

namespace Server { 
    class Program {
        static void Main(string[] args) {
            BaseMessage.Init();
            BaseRoom lobby = BaseRoom.New().Init();
            var sockServer = new TcpListener(System.Net.IPAddress.Parse("0.0.0.0"), 16384);
            sockServer.Start();            
            while (true) {
                Thread.Sleep(100);
                Server.BaseConnection.New().Init( sockServer.AcceptTcpClient(), lobby );
                lobby.Update();
            }
        }
    }
}
