using System.Collections.Generic;
using System.Linq;

namespace Kelson.Advent.Day5
{
    public class QueueStoreSystem : Sys
    {
        public readonly Queue<int> Log = new Queue<int>();
        public readonly Queue<int> Inputs = new Queue<int>();        

        public int Read() => Inputs.Dequeue();

        public void Write(int value) => Log.Enqueue(value);

        private QueueStoreSystem() { }

        public class Device : Sys
        {
            private readonly QueueStoreSystem system;
            public Device(QueueStoreSystem system) => this.system = system;

            public int Read() => system.Log.Dequeue();

            public void Write(int value) => system.Inputs.Enqueue(value);

            public List<int> Buffer => system.Log.ToList();
        }

        public static (Sys system, Device device) CreateDuplux()
        {
            var system = new QueueStoreSystem();
            var device = new Device(system);
            return (system, device);
        }
    }


}
