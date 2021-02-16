using System.ComponentModel.DataAnnotations;

namespace SimulateDownStreamApplication.Model
{
    public class Student
    {
        public int Id
        {
            get; set;
        }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength]
        public string Name
        {
            get; set;
        }

        [Required(ErrorMessage = "Roll number is required")]
        public int RollNumber
        {
            get; set;
        }
    }
}
