using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;

namespace Server{
    public class Client{
        public NetworkStream stream { get; private set; }
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
                    }
                }else{
                    if (this.client.Available >= LengthPrefix) {
                        Serializer.ProcessMessage(this, LengthPrefix);
                        LengthPrefix=0;
                    }                        
                }
            }
        }
        public void Send(MessageBase message) {
            Serializer.Send(this, message);
        }
        public void Close(){
            stream.Close();
            client.Close();
        }
    }
}