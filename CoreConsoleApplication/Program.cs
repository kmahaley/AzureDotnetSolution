using CoreConsoleApplication.DatabaseConcurrency;
using CoreConsoleApplication.Dotnetutilities;
using CoreConsoleApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
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

            var guid = Guid.NewGuid();
            var time = DateTime.Now;
            var item = new Item()
            {
                Id = guid,
                Name = "apple",
                Price = 29,
                CreatedDate = time,
                Tags = new Dictionary<string, string>()
                {
                    { "c", "d" },
                    { "a", "b" },
                }
            };

            var itemGiven = new Item()
            {
                Id = guid,
                Price = 29,
                CreatedDate = time,
                Name = "apple",
                Tags = new Dictionary<string, string>()
                {
                    { "a", "b" },
                    { "c", "d" },
                }
            };

            var comparer = new JTokenEqualityComparer();
            var obj = JToken.Parse(JsonConvert.SerializeObject(item));
            var hashCode = comparer.GetHashCode(obj);

            var objGiven = JToken.Parse(JsonConvert.SerializeObject(itemGiven));
            var hashCodeGiven = comparer.GetHashCode(objGiven);

            Console.WriteLine($"{hashCode}, {hashCodeGiven}");
            Console.WriteLine(hashCode == hashCodeGiven);

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
