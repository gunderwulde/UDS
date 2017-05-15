using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ProtoBuf;
using ProtoBuf.Meta;

public class EntryPoint : MonoBehaviour {
    BaseConnection client;
    // Use this for initialization
    void Start () {
        BaseMessage.Init();

        var tcpClient = new System.Net.Sockets.TcpClient("127.0.0.1", 16384);
        tcpClient.NoDelay = true;
        client = new BaseConnection(tcpClient);

        client.Send(new MessageTest1());
        client.Send(new MessageTest2());
        client.Close();
    }

    // Update is called once per frame
    void Update () {
        client.Process();
    }

}
