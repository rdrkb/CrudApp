﻿using A.Contracts.Update_Models;
using Contracts;
using Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Business.Students
{
    public interface IStudentRepository
    {
        Task CreateNewStudent(StudentModel student);
        Task<List<StudentModel>> GetStudents(int pageNumber, int pageSize, string university, string department);
        Task<StudentModel> GetStudent(string username);
        Task<long> TotalNumberOfStudents(int pageNumber, int pageSize, string university, string department);
        Task<List<UserInfoUpdateEvent>> UpdateStudent(string username, UpdateStudentModel student);
        Task<bool> DeleteStudent(string username);
        Task<bool> DeleteAllStudent();
        Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument);
    }
}
