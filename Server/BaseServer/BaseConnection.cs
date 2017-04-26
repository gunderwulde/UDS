using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;

namespace Server{
    public class BaseConnection{
        public BaseRoom room { get; private set; }

        NetworkStream   stream;
        TcpClient       client;

        public void Init(TcpClient client, BaseRoom room) {
            this.room = room;
            this.client = client;
            this.stream = this.client.GetStream();
            Task.Run(() => Process());
            room.Add(this);
        }

        void Process(){
        int LengthPrefix = 0;
            while (true) {
                try {
                    if (LengthPrefix== 0 ) {
                        if(this.client.Available >=4 ){
                        int fieldNumber;
                            LengthPrefix = ProtoReader.ReadLengthPrefix(stream, false, PrefixStyle.Fixed32, out fieldNumber);
                        }
                    }else{
                        if (this.client.Available >= LengthPrefix) {
                            ProcessMessage(LengthPrefix);
                            LengthPrefix=0;
                        }                        
                    }
                }
                catch (ProtoException e) {
                    Console.WriteLine(">>> " + e);
                    Close();
                }
            }
        }

        public void ProcessMessage(int length) {
                ((BaseMessage)BaseMessage.model.Deserialize(stream, null, typeof(BaseMessage), length, null)).Process(this);
        }

        public void Send(BaseMessage message) {
            try {
                BaseMessage.model.SerializeWithLengthPrefix(stream, message, typeof(BaseMessage), PrefixStyle.Fixed32, 0);
            }
            catch (ProtoException e) {
                Console.WriteLine(">>> " + e);
                Close();
            }
        }

        public void Close() {
            stream.Close();
            client.Close();
            room.Remove(this);
        }
    }
}