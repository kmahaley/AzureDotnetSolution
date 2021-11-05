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

            if(d1.TryGetValue("guid3", out var val)
                && val.Equals("m3"))
            {
                Console.WriteLine("worked");
            }
            

            Console.WriteLine();
        }

        
    }
}