using ProtoBuf;

[ProtoContract]
public class MessageTest1 : BaseMessage {
    [ProtoMember(1)]
    int _data { get; set; }
    [ProtoMember(2)]
    public int secuencia { get; set; }

    public override void Process(BaseConnection client) {
        UnityEngine.Debug.Log(">>>> MessageTest1 "+secuencia);
    }
}
