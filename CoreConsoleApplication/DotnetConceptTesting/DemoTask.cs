using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleApplication.DotnetConceptTesting
{
    public class DemoTask
    {
        public static async Task DemoMethodWithTaskAnOperationCanceledException()
        {
            var cts = new CancellationTokenSource();

            // Simulate cancellation after 1 second
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("after sleeping for 1000");
                cts.Cancel();
            });

            try
            {
                await Task.Delay(5000, cts.Token); // Task.Delay throws TaskCanceledException if canceled
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Task was canceled TaskCanceledException. {ex.CancellationToken.IsCancellationRequested},{cts.IsCancellationRequested}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Task was canceled OperationCanceledException. {ex.CancellationToken.IsCancellationRequested},{cts.IsCancellationRequested}");
            }

            Console.WriteLine("//////////////");
            Console.WriteLine("//////////////");
            Console.WriteLine("//////////////");
            Console.WriteLine("//////////////");
            Console.WriteLine("");

            //////////////////
            cts = new CancellationTokenSource();

            // Simulate cancellation after 1 second
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("after sleeping for 1000");
                //cts.Cancel();
            });

            try
            {
                DoWork(cts.Token);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Task was canceled TaskCanceledException. {ex.Message}, {ex.CancellationToken.IsCancellationRequested},{cts.IsCancellationRequested}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Operation was canceled OperationCanceledException. {ex.Message}, {ex.CancellationToken.IsCancellationRequested}, {cts.IsCancellationRequested}");
            }
        }

        static void DoWork(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                if (token.IsCancellationRequested || i == 0)
                {
                    throw new OperationCanceledException("kartik canceled", token);
                }

                Console.WriteLine($"Working... {i}");
                Thread.Sleep(500);
            }
        }

        private static async Task AsyncTaskWithContinuation()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var token = cts.Token;
            var pingTasks = new List<IPing>()
            {
                new BrokenPing(),
                new WorkingPing(),
                new WorkingPing(),
                new WorkingPing(),
            };

            var tasks = pingTasks.Select(p => p.Ping()).ToList();
            try
            {
                var completedPings = Task.WhenAll(tasks).ContinueWith((task) =>
                {
                    if (task.IsFaulted)
                    {
                        string errMsg = $"Failed to execute task.{task.Exception.Message}, {task.IsFaulted}, {task.IsCanceled}";
                        Console.WriteLine(errMsg);
                        if (task.Exception != null)
                            task.Exception.InnerExceptions.ToList().ForEach(e =>
                            {
                                Console.WriteLine($"Inner exception inside task: {e.Message}");
                            });
                        //throw new Exception(errMsg);
                    }



                }, token);

                await completedPings;

                //completedPings.ToList().ForEach(p => Console.WriteLine($"Ping result: {p}"));
                Console.WriteLine($"all tasks completed. {completedPings.IsFaulted}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error from taks: {ex.Message}");
            }
        }
    }
}
