using BenchmarkDotNet.Running;
using CoreConsoleApplication.BBCProjectUtilities;
using CoreConsoleApplication.Benchmark;
using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var st = Stopwatch.StartNew();
            //var summary = BenchmarkRunner.Run<BechmarkApiDemo>();
            string list = "apple";
            DisplaySet(list);
            await Console.Out.WriteLineAsync($"{list}");
            foreach (int i in list)
            {
                Console.Write(" {0}", i);
            }


            Console.WriteLine("\n Finished main {0}", st.Elapsed.TotalSeconds);

        }

        public static void DisplaySet(string v)
        {
            v = "banana";
            Console.WriteLine($"{v}");
        }

        /// <summary>
        /// Methods can be moved to Main method for utilization. These methods are tested and save
        /// for future use.
        /// </summary>
        public static void ArchievedMethods()
        {
            var fileName = @"C:\Users\kamahale.REDMOND\Downloads\da.csv";
            var pattern = "fabric";
            ReadFileAndReplace.ReadFileAndReplaceString(fileName, pattern);
            DbConcurrencyUtils.CreateDbConcurrenyIssueAndResolution();
            DbConcurrencyUtils.HandleDbContextExceptions();

            SubscriptionUtilities.FindDifferenceInSubscriptionString();
            SubscriptionUtilities.GetCountOfSubscriptionString();

            FrameworkUtilities.GetDotnetFrameworkVersion();
            DotnetDependencies.PrintProjectDependencyTreeUsingMSBuildGraph();
        }
        

        // End of class
    }

    public class Animal { }

    public class Giraffe : Animal { }

}
