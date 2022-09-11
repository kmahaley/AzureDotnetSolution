using System;

namespace CoreWebApplication.Models
{
    public class Item
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreatedDate { get; init; }

        public override bool Equals(object obj)
        {
            return obj is Item item 
                && Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
