using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //await AsyncProgramDemo.startProgram();
            IDictionary<string, string> d1 = new Dictionary<string, string>();
            d1.Add("guid1", "m1");
            d1.Add("guid2", "m2");
            d1.Add("guid3", "m3");
            IDictionary<string, string> d2 = new Dictionary<string, string>();
            d2.Add("guid3", "m31");
            d2.Add("guid4", "m4");
            d2.Add("guid5", "m5");
            IDictionary<string, string> d = null;

            var val = d1?.Values.Where(x => "m5".Equals(x));
            Console.WriteLine($"worked {val.Any()}");

            var myd = d?.Values.Where(x => "m5".Equals(x));
            if(myd?.Any() != true)
            {
                Console.WriteLine("--------------");
            }
            

            Console.WriteLine();
        }

        
    }
}