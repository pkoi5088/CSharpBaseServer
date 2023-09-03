using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public interface IJobQueue
    {
        void Push(Action Job);
    }

    public class JobQueue : IJobQueue
    {
        Queue<Action> _jobQueue = new Queue<Action>();
        object _lock = new object();
        bool _flush = false;

        public void Push(Action Job)
        {
            bool flush = false;

            lock (_lock)
            {
                _jobQueue.Enqueue(Job);
                if (_flush == false)
                    flush = _flush = true;
            }

            if (flush)
                Flush();
        }

        void Flush()
        {
            while (true)
            {
                Action action = Pop();
                if (action == null)
                {
                    _flush = false;
                    return;
                }
                action.Invoke();
            }
        }

        Action Pop()
        {
            if (_jobQueue.Count == 0)
            {
                return null;
            }
            return _jobQueue.Dequeue();
        }
    }
}
