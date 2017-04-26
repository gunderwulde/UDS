using ProtoBuf;

[ProtoContract]
public class MessageTest2 : BaseMessage
{
    [ProtoMember(1)]
    int _data { get; set; }
    public MessageTest2()
    {
        _data = 16384;
    }
    public override void Process(Server.BaseConnection connection)
    {
        System.Console.WriteLine(">>>> SerializableMessage2");
    }
}

