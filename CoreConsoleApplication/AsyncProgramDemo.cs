using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class AsyncProgramDemo
    {
        public static async Task startProgram()
        {
            var methodStopWatch = Stopwatch.StartNew();
            Console.WriteLine($"Start application : {Thread.CurrentThread.ManagedThreadId}");
            var t1 = method1();
            var t2 = method2();
            var t3 = method3();

            // elapseTime = 4099
            //Task.WaitAll(t1, t2);
            // elapseTime = 4099
            //Task.WaitAll(t1, t2, t3);
            // elapseTime = 4099

            //var val1 = await t1;
            //var val2 = await t2;
            //var val3 = await t3;
            //Console.WriteLine($"{val1} {val2} {val3} ");
            //Task.WaitAll(t1, t2, t3);

            //Thread.Sleep(TimeSpan.FromSeconds(6));
            Console.WriteLine($"--- elapseTime = {methodStopWatch.ElapsedMilliseconds}");
            Console.WriteLine();
        }

        public static async Task<string> method1()
        {
            var methodStopWatch = Stopwatch.StartNew();
            await Task.Delay(4000);
            Console.WriteLine($"method1 {Thread.CurrentThread.ManagedThreadId} timespan elapsed = {methodStopWatch.ElapsedMilliseconds}");
            return "m1";
        }

        public static async Task<string> method2()
        {
            var methodStopWatch = Stopwatch.StartNew();
            await Task.Delay(4000);
            Console.WriteLine($"method2 {Thread.CurrentThread.ManagedThreadId} timespan elapsed = {methodStopWatch.ElapsedMilliseconds}");
            return "m2";
        }
        public static async Task<string> method3()
        {
            var methodStopWatch = Stopwatch.StartNew();
            await Task.Delay(4000);
            Console.WriteLine($"method3 {Thread.CurrentThread.ManagedThreadId} timespan elapsed = {methodStopWatch.ElapsedMilliseconds}");
            return "m3";
        }
    }
}
