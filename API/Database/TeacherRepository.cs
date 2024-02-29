using A.Contracts.Update_Models;
using Business;
using Business.Teachers;
using Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClientFactory _mongoClientFactory;

        public TeacherRepository(IConfiguration configuration, MongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
            _configuration = configuration;
        }

        private IMongoCollection<TeacherModel> GetCollection()
        {
            var mongoDatabase = _mongoClientFactory.GetClient(_configuration["MongoDbConfig:ConnectionString"]!).GetDatabase(_configuration["MongoDbConfig:DatabaseName"]);
            return mongoDatabase.GetCollection<TeacherModel>("TeacherDetails");
        }


        public async Task CreateNewTeacher(TeacherModel teacher)
        {
            await GetCollection().InsertOneAsync(teacher);
            return;
        }

        public async Task<bool> DeleteAllTeacher()
        {
            var isDeleted = await GetCollection().DeleteManyAsync(x => true);

            return isDeleted.IsAcknowledged && isDeleted.DeletedCount > 0;
        }

        public async Task<bool> DeleteTeacher(string username)
        {
            var filter = Builders<TeacherModel>.Filter.Eq("Username", username);
            var deleteResult = await GetCollection().DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<TeacherModel> GetTeacher(string username)
        {
            return await GetCollection().Find(teacher => teacher.Username == username).SingleOrDefaultAsync();
        }

        public async Task<List<TeacherModel>> GetTeachers(int pageNumber, int itemPerPage, string university, string department)
        {
            int skipTeacher = (pageNumber - 1) * itemPerPage;
            return await GetCollection().Find(teacher => (university == "all" || teacher.University == university) && (department == "all" || teacher.Department == department))
            .Skip(skipTeacher)
            .Limit(itemPerPage)
            .ToListAsync();
        }

        public async Task<bool> PartiallyUpdateTeacher(string username, JsonPatchDocument<TeacherModel> patchDocument)
        {
            var filter = Builders<TeacherModel>.Filter.Eq("Username", username);

            var findTeacher = await GetCollection().Find(filter).FirstOrDefaultAsync();

            if (findTeacher == null)
            {
                return false;
            }

            patchDocument.ApplyTo(findTeacher);


            var result = await GetCollection().ReplaceOneAsync(filter, findTeacher);

            return result.ModifiedCount > 0;
        }

        public async Task<long> TotalNumberOfTeachers(int pageNumber, int itemPerPage, string university, string department)
        {
            return await GetCollection().Find(teacher => (university == "all" || teacher.University == university) && (department == "all" || teacher.Department == department)).CountDocumentsAsync();
        }

        public async Task<bool> UpdateTeacher(string username, UpdateTeacherModel teacher)
        {
            var currentInfo = Builders<TeacherModel>.Filter.Eq("Username", username);

            var updateInfo = Builders<TeacherModel>.Update
                .Set(s => s.Name, teacher.Name)
                .Set(s => s.University, teacher.University)
                .Set(s => s.Teacher_id, teacher.Teacher_id)
                .Set(s => s.Department, teacher.Department)
                .Set(s => s.Gender, teacher.Gender)
                .Set(s => s.Blood_group, teacher.Blood_group);

            var updateResult = await GetCollection().UpdateOneAsync(currentInfo, updateInfo);

            return updateResult.ModifiedCount > 0;
        }
    }
}
