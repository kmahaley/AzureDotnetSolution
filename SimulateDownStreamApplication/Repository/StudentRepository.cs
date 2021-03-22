using SimulateDownStreamApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulateDownStreamApplication.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private Dictionary<int, Student> dictionary = new Dictionary<int, Student>()
        {
            { 1, new Student { Name="Sachin", RollNumber=111, Id=1 } },
            { 2, new Student { Name="Dina", RollNumber=222, Id=2 } },
            { 3, new Student { Name="Andy", RollNumber=333, Id=3 } }
        };

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
            dictionary[id] = student;
            return dictionary[id];
        }
    }
}