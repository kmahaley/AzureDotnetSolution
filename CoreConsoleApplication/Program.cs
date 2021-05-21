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
            await AsyncProgramDemo.startProgram();
            Console.WriteLine("Completed");
            Console.WriteLine();
        }

        
    }
}