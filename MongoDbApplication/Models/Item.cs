using System;

namespace MongoDbApplication.Models
{
    public class Item
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
