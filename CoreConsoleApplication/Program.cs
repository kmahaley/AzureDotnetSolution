using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //var fileName = @"C:\Users\kamahale.REDMOND\Downloads\da.csv";
            //var pattern = "fabric";
            //ReadFileAndReplace.ReadFileAndReplaceString(fileName, pattern);
            var x = ClusterStatusType.Queued;
            Console.WriteLine($"---{x}");
            Console.WriteLine();
        }

        public enum ClusterStatusType : int
    {
        Queued = 1,
        Creating = 2,
        Running = 3,
        Resizing = 4,
        Terminating = 5,
        Terminated = 6,
        Failed = 7,
        Starting = 8,
    }
        
    }
}