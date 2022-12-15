using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.CSharpUtilities
{
    public static class ProcessDictionary
    {
        /// <summary>
        /// Process dictionary elements parallely and throw exception on a specific key.
        /// Aggreagate rest of the elements which does not throw exception
        /// </summary>
        public static void ProcessDictionaryParallelly()
        {
            ConcurrentDictionary<string, int> dic = new ConcurrentDictionary<string, int>();
            dic.TryAdd("apple", 20);
            dic.TryAdd("banana", 120);
            dic.TryAdd("kiwi", 30);
            dic.TryAdd("pinapple", 240);

            IList<int> listOfElements = new List<int>();
            try
            {
                dic.Keys.AsParallel().ForAll(key =>
                {
                    var x = GetInts(key);
                    listOfElements.Add(x);
                });
            }
            catch (Exception ex)
            {
                HttpRequestException x = ex as HttpRequestException;
                if (x != null)
                {
                    Console.WriteLine(x);
                }
            }

            int agg = AggregateListElements(listOfElements);
            Console.WriteLine(agg);
        }

        private static int AggregateListElements(IList<int> elements)
        {
            Func<int, int, int> aggregateFunc = (x, y) => x + y;
            var agg = elements.Aggregate(0, aggregateFunc);
            return agg;
        }

        private static int GetInts(string s)
        {
            Console.WriteLine($"key received: {s}, {DateAndTime.Now}");
            if (string.Equals("kiwi", s, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("exception =====> ");
                throw new ArgumentNullException($"manual exception on key: {s}");
            }

            return 400;

        }
    }
}
