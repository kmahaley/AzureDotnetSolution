using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.Models
{
    public class Item
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreatedDate { get; init; }

        public IDictionary<string, string> Tags { get; set; }
    }
}
