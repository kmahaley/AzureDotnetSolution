using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var dic = new Dictionary<string, string>();

            var listOfList = new List<Demo>
            {
                { new Demo {name="apple", shorts=new List<string>{ "a", "a", "b" } } },
                { new Demo {name="banana", shorts=new List<string>{ "a", "k", "b" } }},
                { new Demo {name="kiwi", shorts=new List<string>{ "a", "k", "b" } } }
            };
            
            listOfList.ForEach(demo =>
            {
                demo.shorts.ForEach(s => dic.TryAdd(s, demo.name));
            });

            foreach (var kvp in dic)
            {
                Console.WriteLine(kvp.Key +" "+ kvp.Value);

            }

            IEnumerable<string> list = listOfList.SelectMany(d => d.shorts);
            foreach (var s in list)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        
    }

    class Demo
    {
        public string name { get; set; }
        public List<string> shorts { get; set; }
    }
}