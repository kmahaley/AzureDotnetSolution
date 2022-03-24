using CoreConsoleApplication.DatabaseConcurrency;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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
            //DbConcurrencyUtils.CreateDbConcurrenyIssueAndResolution()
            SqlDatabaseContext context = new SqlDatabaseContext();
            var x = context.GetType().Name;

            Console.WriteLine(x);
            Console.WriteLine();
        }

  
        
    }

   
}