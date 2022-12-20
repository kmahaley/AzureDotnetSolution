using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.CSharpUtilities
{
    public static class ComparatorAndComparableUtilities
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
}
