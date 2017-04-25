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

    public void Send(System.IO.Stream stream) {
        Serializer.Send(stream, this);
    }

    public virtual void Process() {
    }
}

