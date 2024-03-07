using A.Contracts.Update_Models;
using Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using NotificationApi.Contracts.Models;

namespace Business.Students.Services
{
    public interface IStudentService
    {
        Task CreateNewStudent(StudentModel student);
        Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department);
        Task<StudentModel> GetStudent(string username);
        Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department);
        Task<List<UpdatedField>> UpdateStudent(string username, UpdateStudentModel student);
        Task<bool> DeleteStudent(string username);
        Task<bool> DeleteAllStudent();
        Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument);
    }
}
