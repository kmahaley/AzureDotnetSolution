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
            DateTime now = DateTime.Now;
            DateTime utc = DateTime.UtcNow;
            DateTime today = DateTime.Today;

            DateTime com = DateTime.Now;

            var x = new DateTime(1604611621646);
            Console.WriteLine($"{now}");
            Console.WriteLine($"{utc}");
            Console.WriteLine($"{today}");

            if(DateTime.Compare(now, com) == 0)
            {
                Console.WriteLine("trueee");
            }
            Console.WriteLine();
        }

        
    }
}