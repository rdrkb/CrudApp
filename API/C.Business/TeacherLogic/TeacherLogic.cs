﻿

using A.Contracts.Models;
using A.Contracts.Update_Models;
using B.Database.MongoDB.StudentData;
using B.Database.MongoDB.TeacherData;
using Microsoft.AspNetCore.JsonPatch;

namespace C.Business.TeacherLogic
{
    public class TeacherLogic : ITeacherLogic
    {
        private readonly ITeacherDataAccess _teacherDataAccess;

        public TeacherLogic(ITeacherDataAccess teacherDataAccess)
        {
            _teacherDataAccess = teacherDataAccess;
        }
        public async Task CreateNewTeacher(TeacherModel teacher)
        {
            await _teacherDataAccess.CreateNewTeacher(teacher);
            return;
        }

        public async Task<bool> DeleteAllTeacher()
        {
            return await _teacherDataAccess.DeleteAllTeacher();
        }

        public async Task<bool> DeleteTeacher(string username)
        {
            return await _teacherDataAccess.DeleteTeacher(username);
        }

        public async Task<TeacherModel> GetTeacher(string username)
        {
            TeacherModel teacherModel = await _teacherDataAccess.GetTeacher(username);
            return teacherModel;
        }

        public async Task<List<TeacherModel>> GetTeachers(int pageNumber, int itemPerPage, string university, string department)
        {
            List<TeacherModel> getTeachers = await _teacherDataAccess.GetTeachers(pageNumber, itemPerPage, university, department);
            return getTeachers;
        }

        public async Task<bool> PartiallyUpdateTeacher(string username, JsonPatchDocument<TeacherModel> patchDocument)
        {
            return await _teacherDataAccess.PartiallyUpdateTeacher(username, patchDocument);
        }

        public async Task<long> TotalNumberOfTeachers(int pageNumber, int itemPerPage, string university, string department)
        {
            long totalTeachers = await _teacherDataAccess.TotalNumberOfTeachers(pageNumber, itemPerPage, university, department);

            return totalTeachers;
        }

        public async Task<bool> UpdateTeacher(string username, UpdateTeacherModel teacher)
        {
            return await _teacherDataAccess.UpdateTeacher(username, teacher);
        }
    }
}
