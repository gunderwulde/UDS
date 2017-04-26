using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProtoBuf;

namespace Server{
    public class BaseRoom{

        List<BaseConnection> connections = new List<BaseConnection>();
        List<BaseConnection> closeds = new List<BaseConnection>();
        DateTime TimeOnStart;

        public BaseRoom(){
            TimeOnStart = DateTime.Now;
        }

        public void Add(BaseConnection connection) {
            lock (connections) {
                connections.Add(connection);
            }
        }

        public void Remove(BaseConnection connection) {
            lock (closeds) {
                closeds.Add(connection);
            }
        }

        public void Update() {
            lock (connections) {
                lock (closeds) {
                    if (closeds.Count > 0) {
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