using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CoreConsoleApplication.DatabaseConcurrency
{
    public static class DbConcurrencyUtils
    {
        public static void CreateDbConcurrenyIssueAndResolution() 
        {
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

            bool isSaved = false;
            while(!isSaved)
            {
                try
                {
                    databaseContext.SaveChanges();
                    isSaved = true;
                    Console.WriteLine("Finally resolved all the conflicts!!!");
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    Console.Write($"Error in database save.{ex.GetType().Name}, count={ex.Entries.Count}, ");
                    foreach(var entry in ex.Entries)
                    {
                        Console.WriteLine($"conflict entity= {entry.Entity.GetType()}");

                        var originalValues = entry.OriginalValues;
                        var currentValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        entry.OriginalValues.SetValues(databaseValues);
                        Console.WriteLine("Database wins!!!");

                        foreach(var property in currentValues.Properties)
                        {
                            var originalValue = originalValues[property];
                            var currentValue = currentValues[property];
                            var databaseValue = databaseValues[property];
                            Console.WriteLine($"App value={currentValue}----- DB ReadValue={originalValue}----- Db present={databaseValue}");

                        }

                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error in database save.{ex.GetType().Name} {ex.Message}");
                }
            }

            Console.WriteLine();
        }
    }
}
