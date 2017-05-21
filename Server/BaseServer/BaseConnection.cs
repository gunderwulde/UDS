using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProtoBuf;

// Ejemplo de sockets asíncromos
// https://msdn.microsoft.com/es-es/library/bew39x2a(v=vs.110).aspx

namespace Server
{
    public class BaseConnection: Pool<BaseConnection> {
        public BaseRoom room { get; private set; }
        NetworkStream   stream;
        TcpClient       client;
        TimeSpan        inicio = new TimeSpan(DateTime.Now.Ticks);
        long            lastKA = DateTime.Now.Ticks; // 1 tick = 1 nanosg
        bool            cerrado = true;
        int             secuencia = 0;

        public BaseConnection Init(TcpClient client, BaseRoom room) {
            this.cerrado = false;
            this.room = room;
            this.client = client;
            this.stream = this.client.GetStream();
            Task.Run(() => Process());
            room.Add(this);
            return this;
        }

        void Process(){
            long currentTicks = DateTime.Now.Ticks;
            int LengthPrefix = 0;
            while (true && !cerrado) {
                Thread.Sleep(0);
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
                   
                    currentTicks = currentTicks = DateTime.Now.Ticks;
                    if ( currentTicks - lastKA > 100000000L )
                    {
                        MessageTest1 msgKA = new MessageTest1
                        {
                            secuencia = secuencia++
                        };                  
                        Send(msgKA);
                        lastKA = currentTicks;
                    }

                }
                catch (ProtoException e) {
                    Console.WriteLine("Process >>> " + e);
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
                Console.WriteLine("ProtoException Send >>> " + e);
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Send >>> " + ex);
                Close();
            }
        }

        public void Close() {
            stream.Close();
            client.Close();
            room.Remove(this);
            this.cerrado = true;
        }
    }
}