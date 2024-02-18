

using A.Contracts.Models;
using A.Contracts.Update_Models;
using B.Database.MongoDB.StudentData;
using B.Database.RedisCache;
using Microsoft.AspNetCore.JsonPatch;

namespace C.Business.StudentLogic
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IStudentDataAccess _studentDataAccess;
        private readonly IRedisCache _redisCache;

        public StudentLogic(IStudentDataAccess studentDataAccess, IRedisCache redisCache)
        {
            _studentDataAccess = studentDataAccess;
            _redisCache = redisCache;
        }

        public async Task CreateNewStudent(StudentModel student)
        {
            await _studentDataAccess.CreateNewStudent(student);
            await _redisCache.RemoveAllData();
            return;
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
            StudentModel studentModel = await _studentDataAccess.GetStudent(username);
            return studentModel;
        }

        public async Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            string cacheKey = pageNumber.ToString() + "-" + itemPerPage.ToString() + university + department;

            var studenetData = await _redisCache.GetData<List<StudentModel>>(cacheKey);

            if (studenetData != null)
            {
                return studenetData;
            }
            else
            {
                List<StudentModel> getStudents = await _studentDataAccess.GetStudents(pageNumber, itemPerPage, university, department);

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

        public async Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            string cacheKey = pageNumber.ToString() + "-" + itemPerPage.ToString() + university + department + "count";

            long totalStudents = await _redisCache.GetData<long>(cacheKey);

            if (totalStudents > 0)
            {
                return totalStudents;
            }
            else
            {
                totalStudents = await _studentDataAccess.TotalNumberOfStudents(pageNumber, itemPerPage, university, department);

                var expiryTime = DateTimeOffset.Now.AddSeconds(20);

                await _redisCache.SetData(cacheKey, totalStudents, expiryTime);

                return totalStudents;
            }
            
        }

        public async Task<bool> UpdateStudent(string username, UpdateStudentModel student)
        {
            await _redisCache.RemoveAllData();
            return await _studentDataAccess.UpdateStudent(username, student);
        }
    }
}
