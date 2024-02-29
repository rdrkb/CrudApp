using A.Contracts.Update_Models;
using Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Business.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentDataAccess;

        public StudentService(IStudentRepository studentDataAccess)
        {
            _studentDataAccess = studentDataAccess;
        }

        public async Task CreateNewStudent(StudentModel student)
        {
            await _studentDataAccess.CreateNewStudent(student);
            return;
        }

        public async Task<bool> DeleteAllStudent()
        {
            return await _studentDataAccess.DeleteAllStudent();
        }

        public async Task<bool> DeleteStudent(string username)
        {
            return await _studentDataAccess.DeleteStudent(username);
        }

        public async Task<StudentModel> GetStudent(string username)
        {
            StudentModel studentModel = await _studentDataAccess.GetStudent(username);
            return studentModel;
        }

        public async Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            return await _studentDataAccess.GetStudents(pageNumber, itemPerPage, university, department);
        }

        public async Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument)
        {
            return await _studentDataAccess.PartiallyUpdateStudent(username, patchDocument);
        }

        public async Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            return await _studentDataAccess.TotalNumberOfStudents(pageNumber, itemPerPage, university, department);
        }

        public async Task<bool> UpdateStudent(string username, UpdateStudentModel student)
        {
            return await _studentDataAccess.UpdateStudent(username, student);
        }
    }
}
