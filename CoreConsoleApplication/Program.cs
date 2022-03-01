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
            SqlDatabaseContext databaseContext = new SqlDatabaseContext();
            var product = databaseContext.Products.Single(p => p.ProductId == 6);
            product.Name = "current";
            product.UnitPrice = 1;
            product.Color = "current";
            product.AvailableQuantity = 100;
            databaseContext.Database.ExecuteSqlRaw("UPDATE dbo.Products SET Name = 'database', Color='database', UnitPrice='1' WHERE ProductId = 6");

            var store = databaseContext.Stores.Single(s => s.StoreId == 1);
            store.StoreName = "current";
            store.City = "current";
            databaseContext.Database.ExecuteSqlRaw("UPDATE dbo.Stores SET StoreName = 'database', City='database' WHERE StoreId = 1");
            
            try
            {
                databaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) //when (ex is DbUpdateConcurrencyException)
            {
                Console.WriteLine($"{ex.Entries.Count}   ---->  Error in database save.{ex.GetType().Name}");
                foreach (var entry in ex.Entries)
                {
                    Console.WriteLine($"{entry.Entity.GetType()}");

                    var originalValues = entry.OriginalValues;
                    var currentValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    foreach (var property in currentValues.Properties)
                    {
                        var originalValue = originalValues[property];
                        var currentValue = currentValues[property];
                        var databaseValue = databaseValues[property];
                        Console.WriteLine($"App value={currentValue}----- DB ReadValue={originalValue}----- Db present={databaseValue}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in database save.{ex.GetType().Name} {ex.Message}");
            }


            Console.WriteLine();
        }

  
        
    }

   
}