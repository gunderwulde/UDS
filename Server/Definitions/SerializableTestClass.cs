using ProtoBuf;
using ProtoBuf.Meta;

[ProtoContract]
public class SerializableMessage1 : SerializableMessageBase {
    [ProtoMember(1)]
    int _data { get; set; }
    public override void Process() {
        System.Console.WriteLine(">>>> SerializableMessage1");
    }
}

[ProtoContract]
public class SerializableMessage2 : SerializableMessageBase { 
    [ProtoMember(1)]
    int _data { get; set; }
    public SerializableMessage2() {
        _data = 16384;
    }
    public override void Process() {
        System.Console.WriteLine(">>>> SerializableMessage2");
    }
}

[ProtoContract]
[ProtoInclude(2, typeof(SerializableMessage1))]
[ProtoInclude(3, typeof(SerializableMessage2))]
public class SerializableMessageBase {
    static RuntimeTypeModel model;

    public static void Init() {
        model = TypeModel.Create();
        model.Add(typeof(SerializableMessage1), true);
        model.Add(typeof(SerializableMessage2), true);
    }

    public static void ProcessMessage(System.IO.Stream stream) {
        SerializableMessageBase test = (SerializableMessageBase)model.Deserialize(stream, null, typeof(SerializableMessageBase));
        test.Process();
    }

    public void Send(System.IO.Stream stream) {
        model.SerializeWithLengthPrefix(stream, this, typeof(SerializableMessageBase), PrefixStyle.Fixed32, 0);
    }

    public virtual void Process() {
    }
}

