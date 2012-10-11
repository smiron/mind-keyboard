using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmoEngineClientLibrary
{
    public class FixedSizedQueue<T>
    {
        public ConcurrentQueue<T> Queue = new ConcurrentQueue<T>();

        public int Limit { get; set; }
        public void Enqueue(T obj)
        {
            Queue.Enqueue(obj);
            lock (this)
            {
                T overflow;
                while (Queue.Count > Limit) Queue.TryDequeue(out overflow);
            }
        }
    }
}
