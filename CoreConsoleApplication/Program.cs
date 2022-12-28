using CoreConsoleApplication.BBCProjectUtilities;
using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var st = Stopwatch.StartNew();
            Console.WriteLine("before delay");

            await Task.Delay(5000);
            
            Console.WriteLine("hello world {0}", st.Elapsed.TotalSeconds);

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
}
