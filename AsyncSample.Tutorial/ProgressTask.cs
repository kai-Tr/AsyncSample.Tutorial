namespace AsyncSample.Tutorial
{
    internal class ProgressTask<T>
    {
        private readonly Action<int> _progressReporter;

        public Task<T?> Task { get; }

        public ProgressTask(Func<T?> function, Action<int> progressReporter)
        {
            _progressReporter = progressReporter;
            Task = new Task<T?>(() => RunWithProgress(function));
        }

        private T? RunWithProgress(Func<T?> function)
        {
            // Initialize progress
            int progress = 0;
            _progressReporter(progress);

            // Simulate work and report progress
            T? result = default;
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(100); // Simulating work
                result = function(); // Execute the function to get result
                progress += 10;
                _progressReporter(progress); // Report progress
            }
            return result;
        }

        public void Start() => Task.Start();
    }
}
