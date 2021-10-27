using System.ComponentModel.DataAnnotations;

namespace SimulateDownStreamApplication.Model
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public int RollNumber { get; set; }
    }
}