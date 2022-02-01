using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public static class ReadFileAndReplace
    {
        public static void ReadFileAndReplaceString(string fileName, string pattern)
        {
            var dictionary = new Dictionary<string, int>();
            int counter = 0;
            // Read the file and display it line by line.  
            foreach(string line in System.IO.File.ReadLines(fileName))
            {
                Console.WriteLine(line+"--------------------------");
                if(line.Contains(pattern))
                {
                    var replacedString = line.Substring(0, line.IndexOf(pattern));
                    Console.WriteLine(replacedString);
                    dictionary.TryAdd(replacedString, 1);
                }
                //var replacedString = Regex.Replace(line, pattern, "", RegexOptions.IgnoreCase);
                //Console.WriteLine(replacedString);
                counter++;
            }
            dictionary.Keys.ToList().ForEach(x => Console.WriteLine(x));
        }
    }
}
