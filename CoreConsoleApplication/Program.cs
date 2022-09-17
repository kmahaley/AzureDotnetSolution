﻿using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public enum ErrorCode 
    {
        ClusterCreationFailed,
        apple,
    }

    public class ErrorDetails
    {
        public string ExceptionDetail { get; set; }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            //var fileName = @"C:\Users\kamahale.REDMOND\Downloads\da.csv";
            //var pattern = "fabric";
            //ReadFileAndReplace.ReadFileAndReplaceString(fileName, pattern);
            //DbConcurrencyUtils.CreateDbConcurrenyIssueAndResolution()
            //FindDifferenceInSubscriptionString();
            //TotalSubscriptionString();
            //DbConcurrencyUtils.HandleDbContextExceptions();

            //FrameworkUtilities.GetDotnetFrameworkVersion();
            //DotnetDependencies.PrintProjectDependencyTreeUsingMSBuildGraph();

            //string str = "";
            //var exceptionDetails = JsonConvert.DeserializeObject<Exception>(str);
            //Console.WriteLine(exceptionDetails);
            var str = " ";
            String.Equals("", "", StringComparison.OrdinalIgnoreCase);
            var errorMessage = $"{ErrorCode.apple} : banana";
            var stringAsJsonSting = JsonConvert.SerializeObject(errorMessage);

            var exception = new Exception(errorMessage);

            //Console.WriteLine(JsonConvert.SerializeObject(str));

            var actualExAsJsonString = JsonConvert.SerializeObject(exception);
            Console.WriteLine(JsonConvert.SerializeObject(exception));

            var errorDetails = new ErrorDetails()
            {
                ExceptionDetail = str,//actualExAsJsonString,
            };

            if (!string.IsNullOrWhiteSpace(errorDetails.ExceptionDetail))
            {
                try
                {
                    var x = JsonConvert.DeserializeObject<Exception>(errorDetails.ExceptionDetail);
                    Console.WriteLine($"after De ===> {x.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }

            Console.WriteLine();

        }

        public static void FindDifferenceInSubscriptionString()
        {
            var originalSubscriptionString = "{CommaSeparatedString}";

            var originalSubscriptionArray = originalSubscriptionString.Split(",");
            List<string> originalSubscription = new List<string>();
            foreach (var item in originalSubscriptionArray)
            {
                originalSubscription.Add(item);
            }

            var newSubscriptionString = "{CommaSeparatedString}";

            var newSubscriptionArray = newSubscriptionString.Split(",");
            List<string> newSubscriptions = new List<string>();
            foreach (var item in newSubscriptionArray)
            {
                newSubscriptions.Add(item);
            }

            var list3 = newSubscriptions.Except(originalSubscription).ToList();
            foreach (var item in list3)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(list3.Count);
        }

        public static void GetCountOfSubscriptionString()
        {
            var originalSubscriptionString = "{CommaSeparatedString}";

            var originalSubscriptionArray = originalSubscriptionString.Split(",");
            ISet<string> originalSubscription = new HashSet<string>();
            foreach (var item in originalSubscriptionArray)
            {
                originalSubscription.Add(item);
            }
            
            Console.WriteLine(originalSubscription.Count);
        }

        // End of class
    }
}
