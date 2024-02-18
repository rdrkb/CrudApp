

using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;

namespace B.Database.MongoDB.TeacherData
{
    public interface ITeacherDataAccess
    {
        Task CreateNewTeacher(TeacherModel teacher);
        Task<List<TeacherModel>> GetTeachers(int pageNumber, int itemPerPage, string university, string department);
        Task<TeacherModel> GetTeacher(string username);
        Task<long> TotalNumberOfTeachers(int pageNumber, int itemPerPage, string university, string department);
        Task<bool> UpdateTeacher(string username, UpdateTeacherModel teacher);
        Task<bool> DeleteTeacher(string username);
        Task<bool> DeleteAllTeacher();
        Task<bool> PartiallyUpdateTeacher(string username, JsonPatchDocument<TeacherModel> patchDocument);
    }
}
