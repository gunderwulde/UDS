using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;

namespace Server{
    public class Client{
        NetworkStream stream;
        TcpClient client;

        public Client(TcpClient client){
            this.client = client;
            this.stream = this.client.GetStream();
            Task.Run( ()=>Process() );             
        }

        void Process(){
        int LengthPrefix = 0;
            while (true) {
                if (LengthPrefix== 0 ) {
                    if(this.client.Available >=4 ){
                    int fieldNumber;
                        LengthPrefix = ProtoReader.ReadLengthPrefix(stream, false, PrefixStyle.Fixed32, out fieldNumber);
                        Console.WriteLine(">>>> LengthPrefix "+LengthPrefix);
                    }
                }else{
                    Console.WriteLine(">>>> LengthPrefix "+LengthPrefix+" Available "+this.client.Available);
                    if (this.client.Available >= LengthPrefix) {
                        Serializer.ProcessMessage(stream);
                        LengthPrefix=0;
                    }                        
                }
            }
        }

        void Close(){
            stream.Close();
            client.Close();
        }
    }
}