using ProtoBuf;

[ProtoContract]
public class MessageTest1 : MessageBase {
    [ProtoMember(1)]
    int _data { get; set; }
    public override void Process(Client client) {
        UnityEngine.Debug.Log(">>>> MessageTest1");
    }
}
