using System;
using System.Collections.Generic;
using System.Linq;

namespace Kelson.Advent.Day5
{
    public class QueueStoreSystem : Sys
    {
        public readonly Queue<int> Log = new Queue<int>();
        public readonly Queue<int> Inputs = new Queue<int>();

        public bool CanRead() => Inputs.Count > 0;

        public int Read()
        {
            var result = Inputs.Dequeue();
            Console.WriteLine($">> read {result} from system");
            return result;
        }

        public void Write(int value)
        {
            Console.WriteLine($">> writing {value} to system");
            Log.Enqueue(value);
        }

        private QueueStoreSystem() { }

        public class Device : Sys
        {            
            private readonly QueueStoreSystem system;
            public Device(QueueStoreSystem system) => this.system = system;

            public bool CanRead() => system.Log.Count > 0;

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
