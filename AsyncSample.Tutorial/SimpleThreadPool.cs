namespace AsyncSample.Tutorial
{
    public class SimpleThreadPool
    {
        private readonly Queue<Action> _tasks = new Queue<Action>();

        public SimpleThreadPool(int numberOfThreads)
        {
            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(new ThreadStart(ProcessTaskQueue))
                {
                    IsBackground = true
                };
                thread.Start();
            }
        }

        public void EnqueueTask(Action task)
        {
            lock (_tasks)
            {
                _tasks.Enqueue(task);
            }
        }

        private void ProcessTaskQueue()
        {
            while (true)
            {
                Action? task = null;
                lock (_tasks)
                {
                    if (_tasks.Count > 0)
                    {
                        task = _tasks.Dequeue();
                    }
                }
                task?.Invoke();
            }
        }
    }
}
