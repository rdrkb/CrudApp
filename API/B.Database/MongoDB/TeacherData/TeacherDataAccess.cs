

using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace B.Database.MongoDB.TeacherData
{
    public class TeacherDataAccess : ITeacherDataAccess
    {
        private readonly IMongoCollection<TeacherModel> _mongoCollection;

        public TeacherDataAccess(IConfiguration configuration)
        {
            // Configure MongoDB
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
            _mongoCollection = mongoDatabase.GetCollection<TeacherModel>("TeacherDetails");
        }

        public async Task CreateNewTeacher(TeacherModel teacher)
        {
            await _mongoCollection.InsertOneAsync(teacher);
            return;
        }

        public async Task<bool> DeleteAllTeacher()
        {
            var isDeleted = await _mongoCollection.DeleteManyAsync(x => true);

            return isDeleted.IsAcknowledged && isDeleted.DeletedCount > 0;
        }

        public async Task<bool> DeleteTeacher(string username)
        {
            var filter = Builders<TeacherModel>.Filter.Eq("Username", username);
            var deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<TeacherModel> GetTeacher(string username)
        {
            return await _mongoCollection.Find(teacher => teacher.Username == username).SingleOrDefaultAsync();
        }

        public async Task<List<TeacherModel>> GetTeachers(int pageNumber, int itemPerPage, string university, string department)
        {
            int skipTeacher = (pageNumber - 1) * itemPerPage;
            return await _mongoCollection.Find(teacher => (university == "all" || teacher.University == university) && (department == "all" || teacher.Department == department))
            .Skip(skipTeacher)
            .Limit(itemPerPage)
            .ToListAsync();
        }

        public async Task<bool> PartiallyUpdateTeacher(string username, JsonPatchDocument<TeacherModel> patchDocument)
        {
            var filter = Builders<TeacherModel>.Filter.Eq("Username", username);

            var findTeacher = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            if (findTeacher == null)
            {
                return false;
            }

            patchDocument.ApplyTo(findTeacher);


            var result = await _mongoCollection.ReplaceOneAsync(filter, findTeacher);

            return result.ModifiedCount > 0;
        }

        public async Task<long> TotalNumberOfTeachers(int pageNumber, int itemPerPage, string university, string department)
        {
            return await _mongoCollection.Find(teacher => (university == "all" || teacher.University == university) && (department == "all" || teacher.Department == department)).CountDocumentsAsync();
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

            var updateResult = await _mongoCollection.UpdateOneAsync(currentInfo, updateInfo);

            return updateResult.ModifiedCount > 0;
        }
    }
}
