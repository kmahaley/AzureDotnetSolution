using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DistributedCacheApplication.Models
{
    public class Employee
    {
        [Required]
        [NotNull]
        public int Id { get; set; }

        [Required]
        [NotNull]
        public string Name { get; set; }

        public int Age { get; set; }

        public Employee(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
    }
}