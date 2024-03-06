using A.Contracts.Update_Models;
using Business.Students.Repositories;
using Contracts;
using Contracts.Models;
using Database.Redis;
using Microsoft.AspNetCore.JsonPatch;

namespace Database
{
    public class StudentCacheRepositoryDecorator : IStudentRepository
    {
        private readonly IStudentRepository _studentDataAccess;
        private readonly IRedisCache _redisCache;

        public StudentCacheRepositoryDecorator(StudentRepository studentDataAccess, IRedisCache redisCache)
        {
            _studentDataAccess = studentDataAccess;
            _redisCache = redisCache;
        }

        public async Task CreateNewStudent(StudentModel student)
        {
            await _studentDataAccess.CreateNewStudent(student);
            await _redisCache.RemoveAllData();
        }

        public async Task<bool> DeleteAllStudent()
        {
            await _redisCache.RemoveAllData();
            return await _studentDataAccess.DeleteAllStudent();
        }

        public async Task<bool> DeleteStudent(string username)
        {
            await _redisCache.RemoveAllData();
            return await _studentDataAccess.DeleteStudent(username);
        }

        public async Task<StudentModel> GetStudent(string username)
        {
            return await _studentDataAccess.GetStudent(username);
        }

        public async Task<List<StudentModel>> GetStudents(int pageNumber, int pageSize, string university, string department)
        {
            string cacheKey = pageNumber.ToString() + "-" + pageSize.ToString() + university + department;

            var studenetData = await _redisCache.GetData<List<StudentModel>>(cacheKey);

            if (studenetData != null)
            {
                return studenetData;
            }
            else
            {
                List<StudentModel> getStudents = await _studentDataAccess.GetStudents(pageNumber, pageSize, university, department);

                var expiryTime = DateTimeOffset.Now.AddMinutes(1);
                var paginationKey = "pages";

                await _redisCache.SetData(cacheKey, getStudents, expiryTime, paginationKey);

                return getStudents;
            }
        }

        public async Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument)
        {
            await _redisCache.RemoveAllData();
            return await _studentDataAccess.PartiallyUpdateStudent(username, patchDocument);
        }

        public async Task<bool> SaveAsync(StudentModel student)
        {
            await _redisCache.RemoveAllData();
            return await _studentDataAccess.SaveAsync(student);
        }

        public async Task<long> TotalNumberOfStudents(int pageNumber, int pageSize, string university, string department)
        {
            string cacheKey = pageNumber.ToString() + "-" + pageSize.ToString() + university + department + "count";

            long totalStudents = await _redisCache.GetData<long>(cacheKey);

            if (totalStudents > 0)
            {
                return totalStudents;
            }
            else
            {
                totalStudents = await _studentDataAccess.TotalNumberOfStudents(pageNumber, pageSize, university, department);

                var expiryTime = DateTimeOffset.Now.AddSeconds(20);

                await _redisCache.SetData(cacheKey, totalStudents, expiryTime);

                return totalStudents;
            }
        }

        public async Task<List<UserInfoUpdateEvent>> UpdateStudent(string username, UpdateStudentModel student)
        {
            await _redisCache.RemoveAllData();
            return await _studentDataAccess.UpdateStudent(username, student);
        }
    }
}
