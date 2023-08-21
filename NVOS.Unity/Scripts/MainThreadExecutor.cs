using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.Unity.Scripts
{
    public class MainThreadExecutor : MonoBehaviour
    {
        private ConcurrentQueue<Action> executionQueue = new ConcurrentQueue<Action>();

        public void Update()
        {
            while (!executionQueue.IsEmpty)
            {
                Action action;
                if (executionQueue.TryDequeue(out action))
                {
                    action.Invoke();
                }
            }
        }

        public void Execute(IEnumerator action)
        {
            executionQueue.Enqueue(() => StartCoroutine(action));
        }

        public void Execute(Action action)
        {
            Execute(WrapAction(action));
        }

        public Task ExecuteAsync(Action action)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void Wrapper()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch(Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Execute(WrapAction(Wrapper));
            return tcs.Task;
        }

        IEnumerator WrapAction(Action a)
        {
            a();
            yield return null;
        }
    }
}
