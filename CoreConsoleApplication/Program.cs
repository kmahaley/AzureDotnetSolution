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

            var person = new Person
            {
                Id = 20,
                FirstName = "apple",
                LastName = "juice"
            };

            Console.WriteLine($"{person.Id}, {person.FirstName} {person.LastName}");

            UpdatePerson(x => 
            {
                person.Id = 40;
                person.FirstName = "updated";
                person.LastName = "changed";
            });

            Console.WriteLine($"{person.Id}, {person.FirstName} {person.LastName}");

            Console.WriteLine();
        }

        public static void UpdatePerson(Action<Person> action)
        {
            var p = new Person
            {
                Id = 2,
                FirstName = "banana",
                LastName = "shake"
            };
            Console.WriteLine($"----------{p.Id}, {p.FirstName} {p.LastName}");
            action(p);
            Console.WriteLine($"-----------{p.Id}, {p.FirstName} {p.LastName}");
        }

  
        
    }

    public class Person
    {
        public int Id { get; set; } // primary key
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}