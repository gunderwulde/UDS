using System;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Server { 
    public class Serializer {
        static RuntimeTypeModel model;
        public static void Init() {
            model = TypeModel.Create();
            model.Add(typeof(MessageTest1), true);
            model.Add(typeof(MessageTest2), true);
        }

        public static void ProcessMessage(Client client, int length) {
            try {
                MessageBase test = (MessageBase)model.Deserialize(client.stream, null, typeof(MessageBase), length, null);
                test.Process();
            }
            catch (ProtoException e) {
                Console.WriteLine(">>> " + e);
            }
        }

        public static void Send(Client client, object message) {
            model.SerializeWithLengthPrefix(client.stream, message, typeof(MessageBase), PrefixStyle.Fixed32, 0);
        }
    }
}