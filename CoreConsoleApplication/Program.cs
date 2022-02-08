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
            //var fileName = @"C:\Users\kamahale.REDMOND\Downloads\da.csv";
            //var pattern = "fabric";
            //ReadFileAndReplace.ReadFileAndReplaceString(fileName, pattern);
            var x = Task.Run(() =>
            {
                Console.WriteLine("before -----------");
                for(int i = 0; i < 3; i++)
                {
                    Console.WriteLine("sleeeeeeeeeeeeeeeeeeeeeeping");
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
                Console.WriteLine("after -----------");
            });
            Console.WriteLine();
        }

        
    }
}