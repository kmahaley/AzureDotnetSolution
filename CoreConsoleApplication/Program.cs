using CoreConsoleApplication.BBCProjectUtilities;
using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using Newtonsoft.Json;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var st = Stopwatch.StartNew();
            //var summary = BenchmarkRunner.Run<BechmarkApiDemo>();

            
            await Console.Out.WriteLineAsync($"Finished main {st.Elapsed.Milliseconds}");
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

}
