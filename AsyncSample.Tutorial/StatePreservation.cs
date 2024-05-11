using System.Threading;

namespace AsyncSample.Tutorial
{
    internal class StatePreservation
    {
        private static AsyncLocal<int> _state = new AsyncLocal<int>();
        public async Task ShowAsyncLocal()
        {
            _state.Value = 42;
            int number = 42;
            await Task.Run(() =>
            {
                // Increment the operation count
                _state.Value++;
                number++;
                Console.WriteLine($"State inside task: {_state.Value}");
                Console.WriteLine($"Number inside task: {number}");
            });

            Console.WriteLine($"\nState after task: {_state.Value}");
            Console.WriteLine($"Number after task: {number}");
        }

        public void ShowAsyncLocalWithExecutionContext()
        {
            ExecutionContext.SuppressFlow();
            _state.Value = 42;
            int number = 42;
            ExecutionContext.RestoreFlow(); // Restore flow
            ExecutionContext executionContext = ExecutionContext.Capture();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                ExecutionContext.Run(executionContext, _ =>
                {
                    // Increment the operation count
                    _state.Value++;
                    number++;
                    Console.WriteLine($"ExecutionContext - SuppressFlow - State inside thread: {_state.Value}");
                    Console.WriteLine($"ExecutionContext - SuppressFlow - Number inside thread: {number}");
                }, null);
            });

            Console.WriteLine($"\nExecutionContext - SuppressFlow - State after thread: {_state.Value}");
            Console.WriteLine($"ExecutionContext - SuppressFlow - Number after thread: {number}");
        }

        public void ShowAsyncLocalWithoutExecutionContext()
        {
            ExecutionContext.SuppressFlow();
            _state.Value = 42;
            int number = 42;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                // Increment the operation count
                _state.Value++;
                number++;
                Console.WriteLine($"ExecutionContext - State inside thread: {_state.Value}");
                Console.WriteLine($"ExecutionContext - Number inside thread: {number}");
            }, null);

            ExecutionContext.RestoreFlow(); // Restore flow
            Console.WriteLine($"\nExecutionContext - State after thread: {_state.Value}");
            Console.WriteLine($"ExecutionContext - Number after thread: {number}");
        }
    }
}
