using CoreConsoleApplication.BBCProjectUtilities;
using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using CoreConsoleApplication.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            

            
            Console.WriteLine();

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
