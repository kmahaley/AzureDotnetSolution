using System;
using System.Linq;
using Microsoft.Data.SqlClient;
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
        
        public static void HandleDbContextExceptions() 
        {
            var p1 = new Product()
            {
                Name = "1",
                AvailableQuantity = 11,
                Color = "blue",
                ProductId = 1,
                UnitPrice = 12,
            };
            
            var p2 = new Product()
            {
                Name = "1",
                AvailableQuantity = 11,
                Color = "blue",
                ProductId = 1,
                UnitPrice = 12,
            };
            try
            {
                using (SqlDatabaseContext databaseContext = new SqlDatabaseContext())
                {
                    databaseContext.Products.Add(p1);
                    databaseContext.SaveChanges();
                }
                using (SqlDatabaseContext databaseContext = new SqlDatabaseContext())
                {
                    databaseContext.Products.Add(p2);
                    databaseContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                while (ex != null)
                {
                    Console.WriteLine(ex.GetType().Name);
                    SqlException sqlEx = ex as SqlException;
                    if (sqlEx == null)
                    {
                        // This is not a SQL exception, go to the next
                        // exception in the chain.
                        ex = ex.InnerException;
                        continue;
                    }

                    foreach (SqlError error in sqlEx.Errors)
                    {
                        if (error.Class == 14 &&
                            (error.Number == 2601 || error.Number == 2627))
                        {
                            // These values are from SQL documentation.
                            // 2601: Class=14, Cannot insert duplicate key row in object
                            // '<Object Name>' with unique index '<Index Name>'
                            // 2627: Class=14, Violation of PRIMARY KEY constraint '%.*ls'.
                            // Cannot insert duplicate key in object '%.*ls'.
                            // http://www.sql-server-helper.com/error-messages/msg-2601.aspx
                            Console.WriteLine("true");
                        }
                    }

                    // Error code not found in this SQL Exception, try the next
                    // exception in the chain.
                    ex = ex.InnerException;
                }

                
                Console.WriteLine(ex);
            }

            Console.WriteLine();
        }
    }
}
