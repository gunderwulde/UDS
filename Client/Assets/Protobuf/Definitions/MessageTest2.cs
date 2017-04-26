using ProtoBuf;

[ProtoContract]
public class MessageTest2 : MessageBase { 
    [ProtoMember(1)]
    int _data { get; set; }
    public MessageTest2() {
        _data = 16384;
    }

    public override void Process(Client client) {
        UnityEngine.Debug.Log(">>>> MessageTest2");
    }
}

