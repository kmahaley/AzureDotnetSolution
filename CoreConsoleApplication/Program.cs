using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Exception e1 = new ArgumentException();

            Exception e2 = new ArgumentException();
            List<Exception> list = new List<Exception>() { new ArgumentNullException(), new ArgumentOutOfRangeException() };

            if(list.Any(ex => ex.GetType() == e1.GetType()))
            {

                Console.WriteLine("==================== equal");
            }
            else
            {
                Console.WriteLine("nooooooooooooooooooooooo");
            }
        }
    }
}
