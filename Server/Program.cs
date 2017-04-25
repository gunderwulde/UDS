using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;

namespace Server { 
    class Program {
        static void Main(string[] args) {
            SerializableMessageBase.Init();
            var sockServer = new TcpListener(System.Net.IPAddress.Parse("0.0.0.0"), 16384);
            sockServer.Start();            
            while (true) {
                NewClient( sockServer.AcceptTcpClient() );
            }
        }

        public static async void NewClient(TcpClient client) {
            await Task.Run(() => {
                NetworkStream stream = client.GetStream();
                while (true) {
                    if (stream.DataAvailable) {
                        int fieldNumber;
                        int LengthPrefix = ProtoReader.ReadLengthPrefix(stream, false, PrefixStyle.Fixed32, out fieldNumber);
                        Console.WriteLine("client.Available: " + client.Available + " " + LengthPrefix);
                        if (client.Available >= LengthPrefix) {
                            SerializableMessageBase.ProcessMessage(stream);
                        }                        
                    }
                }
                stream.Close();
                client.Close();
            });
        }
    }
}
