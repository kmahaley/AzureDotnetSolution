using BenchmarkDotNet.Disassemblers;
using CoreConsoleApplication.BBCProjectUtilities;
using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using CoreConsoleApplication.Models;
using DequeNet;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;
using static BenchmarkDotNet.Engines.EngineEventSource;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var st = Stopwatch.StartNew();
            //var summary = BenchmarkRunner.Run<BechmarkApiDemo>();

            var min = 0;
            var max = 20;
            var newDeque = new Deque<int>();
            for (int index = max; index >= min; index--)
            {

                if (index % 2 == 0)
                {
                    newDeque.PushRight(index);
                }
                else
                {
                    newDeque.PushLeft(index);
                    
                }
            }

            for (int index = min; index <= max; index++)
            {
                Console.Write($"{newDeque[index]}-");
            }

            await Console.Out.WriteLineAsync($"Finished main. time:{st.Elapsed.Seconds}secs");
        }

        


        //{"node-agent.fail-node-service-failure.enabled": false, "vm-generation-based-cluster-creation.enabled":true}
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

            TaskBasedUtilities.HandleTaskAsync();
        }

        public static void PrintList(IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        // End of class
    }

    public class Animal { }

    public class Giraffe : Animal
    {
        public int No { get; set; }
        public string Name { get; set; }
        public List<string> ListOfRegion { get; set; }
    }

    public class Giraffe1 : Animal
    {
        public int No { get; set; }
        public List<string> ListOfRegion { get; set; }
    }

    public enum VnetSecurity 
    {
        Shared,
        Managed
    }
}
