using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;

public class Serializer {
    static RuntimeTypeModel model;

    public static void Init() {
        model = TypeModel.Create();
        model.Add(typeof(SerializableMessage1), true);
        model.Add(typeof(SerializableMessage2), true);
    }

    public static void ProcessMessage(Stream stream) {
        SerializableMessageBase test = (SerializableMessageBase)model.Deserialize(stream, null, typeof(SerializableMessageBase));
        test.Process();
    }

    public static void Send(Stream stream, object message){
        model.SerializeWithLengthPrefix(stream, message, typeof(SerializableMessageBase), PrefixStyle.Fixed32, 0);
    }

}

