using UnityEngine;
using System.Net.Sockets;
using ProtoBuf;
using ProtoBuf.Meta;

public class Client {
    public NetworkStream stream { get; private set; }
    TcpClient client;

    public Client(TcpClient client) {
        this.client = client;
        this.stream = this.client.GetStream();
    }

    int LengthPrefix = 0;
    public void Process() {
        if (LengthPrefix == 0) {
            if (this.client.Available >= 4) {
                int fieldNumber;
                LengthPrefix = ProtoReader.ReadLengthPrefix(stream, false, PrefixStyle.Fixed32, out fieldNumber);
            }
        }
        else {
            if (this.client.Available >= LengthPrefix) {
                Serializer.ProcessMessage(this, LengthPrefix);
                LengthPrefix = 0;
            }
        }
    }

    public void Send(MessageBase message) {
        Serializer.Send(this, message);
    }

    public void Close() {
        stream.Close();
        client.Close();
    }
}
