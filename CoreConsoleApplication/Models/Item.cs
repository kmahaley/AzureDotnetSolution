using System;
using System.Collections.Generic;

namespace CoreConsoleApplication.Models
{
    public class Item : IComparable<Item>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreatedDate { get; init; }

        public IDictionary<string, string> Tags { get; set; }
        
        public Item() { }

        public Item(Guid id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public int CompareTo(Item other)
        {
            return Price.CompareTo(other.Price);
        }

        public override string ToString() 
        {
            return $"{Id} : {Name} : {Price}";
        }
    }
}
