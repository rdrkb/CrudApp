
using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace B.Database.MongoDB.StudentData
{
    public class StudentDataAccess : IStudentDataAccess
    {
        private readonly IMongoCollection<StudentModel> _mongoCollection;

        public StudentDataAccess(IConfiguration configuration)
        {
            // Configure MongoDB
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
            _mongoCollection = mongoDatabase.GetCollection<StudentModel>("StudentDetails");
        }
        public async Task CreateNewStudent(StudentModel student)
        {
            await _mongoCollection.InsertOneAsync(student);
            return;
        }

        public async Task<bool> DeleteAllStudent()
        {
            var isDeleted = await _mongoCollection.DeleteManyAsync(x => true);

            return isDeleted.IsAcknowledged && isDeleted.DeletedCount > 0;
        }

        public async Task<bool> DeleteStudent(string username)
        {
            var filter = Builders<StudentModel>.Filter.Eq("Username", username);
            var deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<StudentModel> GetStudent(string username)
        {
            return await _mongoCollection.Find(student => student.Username == username).SingleOrDefaultAsync();
        }

        public async Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            int skipStudent = (pageNumber - 1) * itemPerPage;
            return await _mongoCollection.Find(student => (university == "all" || student.University == university) && (department == "all" || student.Department == department))
            .Skip(skipStudent)
            .Limit(itemPerPage)
            .ToListAsync();
        }

        public async Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument)
        {
            var filter = Builders<StudentModel>.Filter.Eq("Username", username);

            var findStudent = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            if (findStudent == null)
            {
                return false;
            }

            patchDocument.ApplyTo(findStudent);


            var result = await _mongoCollection.ReplaceOneAsync(filter, findStudent);

            return result.ModifiedCount > 0;
        }

        public async Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            return await _mongoCollection.Find(student => (university == "all" || student.University == university) && (department == "all" || student.Department == department)).CountDocumentsAsync();
        }

        public async Task<bool> UpdateStudent(string username, UpdateStudentModel student)
        {
            var currentInfo = Builders<StudentModel>.Filter.Eq("Username", username);

            var updateInfo = Builders<StudentModel>.Update
                .Set(s => s.Name, student.Name)
                .Set(s => s.University, student.University)
                .Set(s => s.Student_id, student.Student_id)
                .Set(s => s.Department, student.Department)
                .Set(s => s.Gender, student.Gender)
                .Set(s => s.Year_of_graduation, student.Year_of_graduation)
                .Set(s => s.Blood_group, student.Blood_group);

            var updateResult = await _mongoCollection.UpdateOneAsync(currentInfo, updateInfo);

            return updateResult.ModifiedCount > 0;
        }
    }
}
