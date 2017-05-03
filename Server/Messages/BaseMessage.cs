using ProtoBuf;
using ProtoBuf.Meta;

[ProtoContract]
[ProtoInclude(2, typeof(MessageTest1))]
[ProtoInclude(3, typeof(MessageTest2))]
public class BaseMessage {
    public virtual void Process(Server.BaseConnection connection) { }

    public static RuntimeTypeModel model;
    public static void Init()
    {

        model = TypeModel.Create();
        model.Add(typeof(MessageTest1), true);
        model.Add(typeof(MessageTest2), true);
    }

}
