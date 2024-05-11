using System.Threading.Tasks;

namespace AsyncSample.Tutorial
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Async Tutorial");

            Console.WriteLine("Access Web\n");
            await AccessWebAsync();

            Console.WriteLine("\nSimple Thread Pool\n");
            SimpleThreadPool();

            Console.WriteLine("\nAsyncLocal and ExecutionContext\n");
            await StatePreservation();

            // Usage example
            //Console.WriteLine("\nImplementing a Custom Task with Progress Reporting\n");
            //ProgressTask<int> progressTask = new ProgressTask<int>(
            //    function: () => new Random().Next(1, 100),
            //    progressReporter: progress => Console.WriteLine($"Progress: {progress}%"));

            //progressTask.Start();

            //Console.WriteLine("Task result: " + progressTask.Task.Result);

            //Console.WriteLine("\nAdvanced Async Patterns and Error Handling in C#\n");
            //await ProcessTasksAsync();

            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }

        static async Task AccessWebAsync()
        {
            HttpClient client = new();
            var result = await client.GetStringAsync("http://exmaple.com");
            Console.WriteLine(result);
        }

        static void SimpleThreadPool()
        {
            var threadPool = new SimpleThreadPool(2);
            threadPool.EnqueueTask(async () =>
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Simple Thread Pool - Task enqueued 1.\n");
                });
            });

            threadPool.EnqueueTask(async () =>
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Simple Thread Pool - Task enqueued 2.\n");
                });
            });
        }

        static async Task StatePreservation()
        {
            var statePreservation = new StatePreservation();
            await statePreservation.ShowAsyncLocal();
            statePreservation.ShowAsyncLocalWithExecutionContext();
            statePreservation.ShowAsyncLocalWithoutExecutionContext();
        }

        static async Task ProcessTasksAsync()
        {
            var tasks = new List<Task>();
            try
            {
                tasks.Add(Task.Run(() => Console.WriteLine("Task 1")));
                tasks.Add(Task.Run(() => Console.WriteLine("Task 2")));
                tasks.Add(Task.Run(() => throw new Exception("Oops!")));

                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                // Handle errors once all tasks have completed
                foreach (var task in tasks.Where(t => t.IsFaulted))
                {
                    Console.WriteLine($"Error fetching data: {task.Exception?.InnerException?.Message}");
                }
            }
        }
    }
}
