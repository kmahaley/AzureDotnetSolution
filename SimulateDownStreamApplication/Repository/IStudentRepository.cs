using SimulateDownStreamApplication.Model;
using System.Collections.Generic;

namespace SimulateDownStreamApplication.Repository
{
    public interface IStudentRepository
    {
        public Student AddStudent(Student student);

        public Student GetStudent(int ind);

        public List<Student> GetStudents();
        
        public Student UpdateStudent(int id, Student student);

        public Student PatchStudent(int id, Student student);

        public bool DeleteStudent(int id);

        
    }
}
