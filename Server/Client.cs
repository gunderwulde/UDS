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
            int lengthPrefix = 0;
            while (true) {
                if (lengthPrefix== 0 ) {
                    if(this.client.Available >=4 ){
                    int fieldNumber;
                        lengthPrefix = ProtoReader.ReadLengthPrefix(stream, false, PrefixStyle.Fixed32, out fieldNumber);
                    }
                }else{
                    if (this.client.Available >= lengthPrefix) {
                        Serializer.ProcessMessage(stream);
                        lengthPrefix=0;
                    }                        
                }
            }
        }

        public void Close(){
            stream.Close();
            client.Close();
        }
    }
}