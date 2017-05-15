using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProtoBuf;

namespace Server{
    public class BaseRoom: Pool<BaseRoom>{
        List<BaseConnection> connections = new List<BaseConnection>();
        List<BaseConnection> closeds = new List<BaseConnection>();
        DateTime TimeOnStart;

        public BaseRoom Init() {
            TimeOnStart = DateTime.Now;
            return this;
        }

        public void Add(BaseConnection connection) {
            lock (connections) {
                connections.Add(connection);
                System.Console.WriteLine(">>>> Room new connection " + connections.Count);
            }
        }

        public void Remove(BaseConnection connection) {
            lock (closeds) {
                closeds.Add(connection);
            }
        }

        public void Update() {
            if (closeds.Count > 0) {
                lock (connections) {
                   lock (closeds) {                    
                        foreach (var connection in closeds)
                            connections.Remove(connection);
                        closeds.Clear();
                    }
                }
            }
        }

        public void Broadcast(BaseMessage message, BaseConnection exception = null) {
            lock (connections) {
                foreach (var conexion in connections)
                    if (conexion != exception)
                        conexion.Send(message);
            }
        }

        public float Clock() {
            TimeSpan span = DateTime.Now.Subtract(TimeOnStart);
            return (float)(span.TotalMilliseconds / 1000.0);
        }

    }
}