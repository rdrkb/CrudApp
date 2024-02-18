

using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;

namespace C.Business.StudentLogic
{
    public interface IStudentLogic
    {
        Task CreateNewStudent(StudentModel student);
        Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department);
        Task<StudentModel> GetStudent(string username);
        Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department);
        Task<bool> UpdateStudent(string username, UpdateStudentModel student);
        Task<bool> DeleteStudent(string username);
        Task<bool> DeleteAllStudent();
        Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument);
    }
}
