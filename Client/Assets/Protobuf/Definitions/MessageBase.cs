using ProtoBuf;

[ProtoContract]
[ProtoInclude(2, typeof(MessageTest1))]
[ProtoInclude(3, typeof(MessageTest2))]
public class MessageBase {
    public virtual void Process(Client client) { }
}

