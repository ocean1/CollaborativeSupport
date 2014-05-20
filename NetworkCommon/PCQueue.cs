using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtils
{
    /// <summary>
    /// a producer/consumer queue implementation,
    /// we use the generic System.Action delegate
    /// to queue a "job" that will get executed in
    /// the worker thread context
    /// </summary>
    public class PCQueue
    {
        private readonly object _locker = new object();
        private Thread[] _workers;
        private Queue<Action> _itemQ = new Queue<Action>();

        public bool IsEmpty
        {
            get
            {
                lock (_locker)
                {
                    return (_itemQ.Count == 0);
                }
            }

        }

        public PCQueue(int workerCount)
        {
            _workers = new Thread[workerCount];

            // Create and start a separate thread for each worker 
            for (int i = 0; i < workerCount; i++)
                (_workers[i] = new Thread(Consume)).Start();
        }

        public void Close(bool waitForWorkers)
        {
            // Enqueue one null item per worker to make each exit. 
            foreach (Thread worker in _workers)
                EnqueueItem(null);

            // Wait for workers to finish 
            if (waitForWorkers)
                foreach (Thread worker in _workers)
                    worker.Join();
        }

        public void EnqueueItem(Action item)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(item);           // We must pulse because we're 
                Monitor.Pulse(_locker);         // changing a blocking condition. 
            }
        }

        void Consume()
        {

            while (true)                        // Keep consuming until 
            {                                   // told otherwise. 
                Action item;
                lock (_locker)
                {
                    while (_itemQ.Count == 0) Monitor.Wait(_locker);
                    item = _itemQ.Dequeue();
                }
                if (item == null) return;         // This signals our exit. 

                item();                           // Execute item.
            }
        }

    }
}
