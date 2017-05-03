using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server{
    public class Pool<T>: IDisposable where T : class, new() {
        static Stack<IDisposable> stack = new Stack<IDisposable>();
        public static T New() {
            if (stack.Count == 0) {
                return new T();
            }
            return (T)stack.Pop();
        }

        public void Dispose() {
            stack.Push(this);
        }

    }
}
