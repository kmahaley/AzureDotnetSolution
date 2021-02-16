using SimulateDownStreamApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimulateDownStreamApplication.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private Dictionary<int, Student> dictionary = new Dictionary<int, Student>();


        public Student AddStudent(Student student)
        {
            if (dictionary.ContainsKey(student.Id))
            {
                throw new Exception("student already exists");
            }
            dictionary[student.Id] = student;
            return student;
        }

        public bool DeleteStudent(int id)
        {
            return dictionary.Remove(id);
        }

        public Student GetStudent(int id)
        {
            if (dictionary.ContainsKey(id))
            {
                return dictionary[id];
            }
            throw new Exception("Student not present");
        }

        public List<Student> GetStudents()
        {
            return dictionary.Values.ToList();
        }

        public Student PatchStudent(int id, Student student)
        {
            return UpdateStudent(id, student);
        }

        public Student UpdateStudent(int id, Student student)
        {
            if (!dictionary.ContainsKey(id))
            {
                throw new Exception("Student not present");
            }
            student.Id = id;
            dictionary[id] =  student;
            return dictionary[id];
        }
    }

}
