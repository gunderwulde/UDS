using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ProtoBuf;
using ProtoBuf.Meta;

public class EntryPoint : MonoBehaviour {
	// Use this for initialization
	void Start () {
        SerializableMessageBase.Init();

        //Listener();
        var client = new System.Net.Sockets.TcpClient("127.0.0.1", 16384);
        System.Net.Sockets.NetworkStream stream = client.GetStream();

        new SerializableMessage1().Send(stream);
//        new SerializableMessage2().Send(stream);

        stream.Close();
        client.Close();
    }

    // Update is called once per frame
    void Update () {
    }

}
