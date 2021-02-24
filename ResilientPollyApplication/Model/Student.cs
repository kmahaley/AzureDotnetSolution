using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Model
{
    // Student model is mapped to SimulatedDownStreamApplication.Model.Student class
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RollNumber { get; set; }
    }

}

