using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XamarinTSP.Common
{
    public class DelegateInvocationQueue
    {
        private static volatile object _lck = new object();
        public int MillisecondsTimeout { get; set; }
        private bool _isRunning;
        private bool _isStopped;
        private Queue<Delegate> _queue = new Queue<Delegate>();

        public void ClearQueue()
        {
            _isStopped = false;
            _queue.Clear();
        }
        public async Task InvokeNext(Delegate action, params object[] parameters)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(MillisecondsTimeout);
                _isStopped = false;
                lock (_lck)
                {
                    if (!_isStopped)
                    {
                        _queue.Enqueue(action);
                        if (!_isRunning)
                        {
                            _isRunning = true;

                            var nextAction = _queue.Dequeue();
                            nextAction.DynamicInvoke(parameters);
                            _isRunning = false;
                        }
                    }
                }
            });
        }


    }
}
