using BenchmarkDotNet.Disassemblers;

using CoreConsoleApplication.BBCProjectUtilities;
using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.DotnetConceptTesting;
using CoreConsoleApplication.Dotnetutilities;
using CoreConsoleApplication.Models;

using DequeNet;

using Iced.Intel;

using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Microsoft.WSMan.Management;

using Newtonsoft.Json;

using NuGet.ContentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Security.Principal;
using System.Text.Json;
using System.Threading;
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

            

            await Console.Out.WriteLineAsync($"Finished main. time:{st.Elapsed.Seconds}secs");

        }


        enum Status { Invalid = 0, Active = 1, Inactive = 2 }


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

            DemoTask.DemoMethodWithTaskAnOperationCanceledException();
            //await DemoTask.AsyncTaskWithContinuation();
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

    public enum RuleDirection
    {
        Inbound,
        Outbound,

    }

    public interface IPing
    {
        Task<bool> Ping();
    }

    public class WorkingPing : IPing
    {
        public async Task<bool> Ping()
        {
            await Task.Delay(3000);
            Console.WriteLine("success....");
            return true;
        }
    }

    public class BrokenPing : IPing
    {
        public async Task<bool> Ping()
        {
            throw new Exception("Ping Broken....");
        }
    }
}
