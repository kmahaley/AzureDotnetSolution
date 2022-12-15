using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.BBCProjectUtilities
{
    public static class SubscriptionUtilities
    {
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

            var differenceInTwoList = newSubscriptions.Except(originalSubscription).ToList();
            foreach (var subscription in differenceInTwoList)
            {
                Console.WriteLine(subscription);
            }
            Console.WriteLine(differenceInTwoList.Count);
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
    }
}
