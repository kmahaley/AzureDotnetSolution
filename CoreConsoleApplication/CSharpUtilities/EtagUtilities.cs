using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreConsoleApplication.Models;

namespace CoreConsoleApplication.CSharpUtilities
{
    public static class EtagUtilities
    {
        public static void GetHashCodeUsingJTokenEqualityComparer()
        {
            var guid = Guid.NewGuid();
            var time = DateTime.Now;
            var item = new Item()
            {
                Id = guid,
                Name = "apple",
                Price = 29,
                CreatedDate = time,
                Tags = new Dictionary<string, string>()
                {
                    { "c", "d" },
                    { "a", "b" },
                }
            };

            var itemGiven = new Item()
            {
                Id = guid,
                Price = 29,
                CreatedDate = time,
                Name = "apple",
                Tags = new Dictionary<string, string>()
                {
                    { "a", "b" },
                    { "c", "d" },
                }
            };

            var comparer = new JTokenEqualityComparer();
            var obj = JToken.Parse(JsonConvert.SerializeObject(item));
            var hashCode = comparer.GetHashCode(obj);

            var objGiven = JToken.Parse(JsonConvert.SerializeObject(itemGiven));
            var hashCodeGiven = comparer.GetHashCode(objGiven);

            Console.WriteLine($"{hashCode}, {hashCodeGiven}");
            Console.WriteLine(hashCode == hashCodeGiven);
        }
    }

    
}
