using ProtoBuf;

[ProtoContract]
public class MessageTest1 : MessageBase
{
    [ProtoMember(1)]
    int _data { get; set; }
    public override void Process()
    {
        System.Console.WriteLine(">>>> SerializableMessage1");
    }
}