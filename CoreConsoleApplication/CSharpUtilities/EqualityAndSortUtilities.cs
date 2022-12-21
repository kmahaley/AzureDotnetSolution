using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.CSharpUtilities
{
    public static class EqualityAndSortUtilities
    {
        public static void DemonstrateComparable() 
        {
            var numbers = new List<Book>
            {
                new Book(Guid.NewGuid(), "pineapple", 152),
                new Book(Guid.NewGuid(), "banana", 2),
                new Book(Guid.NewGuid(), "kiwi", 15),
                new Book(Guid.NewGuid(), "apple", 10),
                new Book(Guid.NewGuid(), "mango", 500),
            };

            Console.WriteLine("before sort.");
            numbers.ForEach(item => Console.WriteLine(item));

            numbers.Sort();
            Console.WriteLine("after sort. IComparable => \n \n");
            numbers.ForEach(item => Console.WriteLine(item));

        }

        public static void DemonstrateIComparer()
        {
            var numbers = new List<Book>
            {
                new Book(Guid.NewGuid(), "pineapple", 152),
                new Book(Guid.NewGuid(), "banana", 2),
                new Book(Guid.NewGuid(), "kiwi", 15),
                new Book(Guid.NewGuid(), "apple", 10),
                new Book(Guid.NewGuid(), "mango", 500),
            };

            Console.WriteLine("before sort.");
            numbers.ForEach(item => Console.WriteLine(item));

            numbers.Sort(new BookComparer());
            Console.WriteLine("after sort. IComparer => \n \n");
            numbers.ForEach(item => Console.WriteLine(item));
        }

        public static void DemonstrateIEqualityComparer()
        {
            var numberDictionary = new Dictionary<Book, int>(new BookEqualityComparer())
            {
                { new Book(Guid.NewGuid(), "pineapple", 152), 152 },
                { new Book(Guid.NewGuid(), "banana", 2), 2 },
                { new Book(Guid.NewGuid(), "kiwi", 15), 15 },
                { new Book(Guid.NewGuid(), "apple", 10), 10 },
                { new Book(Guid.NewGuid(), "mango", 500), 500 }, 
            };

            Console.WriteLine("before sort.");
            foreach ( var kv in numberDictionary)
            {
                Console.WriteLine($"{kv.Key} => {kv.Value}");
            }

            var isAdded = numberDictionary.TryAdd(new Book(Guid.NewGuid(), "mango", 5000), 5000);
            Console.WriteLine(isAdded);
        }

        public static void DemonstrateIEquatable()
        {
            var numberDictionary = new Dictionary<Box, int>()
            {
                { new Box(10, 1, 2), 152 },
                { new Box(2, 6, 2), 2 },
                { new Box(10, 1, 1), 15 },
                { new Box(5, 2, 2), 10 },
                { new Box(4, 5, 1), 500 },
            };

            foreach (var kv in numberDictionary)
            {
                Console.WriteLine($"{kv.Key} => {kv.Value}");
            }

            var isAdded = numberDictionary.TryAdd(new Box(4, 5, 1), 5000);
            Console.WriteLine("added? {0}", isAdded);
            
            var contains = numberDictionary.ContainsKey(new Box(4, 5, 1));
            Console.WriteLine("contains? {0}", contains);

            foreach (var kv in numberDictionary)
            {
                Console.WriteLine($"{kv.Key} => {kv.Value}");
            }

            Console.WriteLine("done!");
        }
    }

    public class Book : IComparable<Book>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public int Price { get; set; }

        public Book() { }

        public Book(Guid id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public int CompareTo(Book other)
        {
            return Price.CompareTo(other.Price);
        }

        public override string ToString()
        {
            return $"{Id} : {Name} : {Price}";
        }
    }

    public class BookComparer : IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            int xLength = x.Name.Length;
            int yLength = y.Name.Length;

            if (xLength > yLength)
                return 1;
            if (xLength < yLength)
                return -1;

            return 0;
        }
    }


    public class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals(Book x, Book y)
        {
            var isEqual = x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase);
            
            //var isEqual = x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase)
            //    && x.Price == y.Price
            //    && Id.Equals(y.Id);
            
            return isEqual;
        }

        public int GetHashCode(Book book)
        {
            return HashCode.Combine(book.Name);
            //return HashCode.Combine(book.Id, book.Name, book.Price);
        }
    }

    public class Box : IEquatable<Box>
    {
        public int Height { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public Box(int h, int l, int w)
        {
            Height = h;
            Length = l;
            Width = w;
        }

        public bool Equals(Box other)
        {
            if(ReferenceEquals(this, other))
                return true;
            if(this is null || other is null)
                return false;

            return Height == other.Height
                && Length == other.Length
                && Width == other.Width;
        }

        public override bool Equals(object obj)
        {
            var box = obj as Box;
            return Equals(box);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Height, Length, Width);
        }

        public override string ToString()
        {
            return $"{Height} : {Length} : {Width}";
        }
    }
}
