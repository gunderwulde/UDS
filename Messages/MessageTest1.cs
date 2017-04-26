using ProtoBuf;

[ProtoContract]
public class MessageTest1 : BaseMessage
{
    [ProtoMember(1)]
    int _data { get; set; }
    public override void Process(Server.BaseConnection connection)
    {
        System.Console.WriteLine(">>>> SerializableMessage1");
    }
}