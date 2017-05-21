using UnityEngine;
using System.Net.Sockets;
using ProtoBuf;
using ProtoBuf.Meta;

public class BaseConnection {
    public NetworkStream stream { get; private set; }
    TcpClient client=null;

    public BaseConnection(TcpClient client) {
        this.client = client;
        this.stream = this.client.GetStream();
    }

    int LengthPrefix = 0;
    public void Process() {
        try {
            if (client == null || !client.Connected)
                return;
            if (LengthPrefix == 0) {
                if (this.client.Available >= 4) {
                    int fieldNumber;
                    LengthPrefix = ProtoReader.ReadLengthPrefix(stream, false, PrefixStyle.Fixed32, out fieldNumber);
                }
            }
            else {
                if (this.client.Available >= LengthPrefix) {
                    ProcessMessage(LengthPrefix);
                    LengthPrefix = 0;
                }
            }
        }
        catch (ProtoException e) {
            Debug.Log("Recv ERROR >>> " + e);
            Close();
        }
    }

    public void ProcessMessage(int length) {
        ((BaseMessage)BaseMessage.model.Deserialize(stream, null, typeof(BaseMessage), length, null)).Process(this);
    }

    public void Send(BaseMessage message)
    {
        try {
            BaseMessage.model.SerializeWithLengthPrefix(stream, message, typeof(BaseMessage), PrefixStyle.Fixed32, 0);
        }
        catch (ProtoException e) {
            Debug.Log("Send ERROR >>> " + e);
            Close();
        }
    }

    public void Close() {
        stream.Close();
        stream.Dispose();
        client.Close();
        stream = null;
        client = null;
        // TODO No se detecta el cierre en el servidor
        
    }
}
