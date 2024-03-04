using A.Contracts.Update_Models;
using Business.Students;
using Contracts;
using Contracts.Models;
using Contracts.MongoClientFactory;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClientFactory _mongoClientFactory;

        public StudentRepository(IConfiguration configuration, MongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
            _configuration = configuration;
        }

        private IMongoCollection<StudentModel> GetCollection()
        {
            var mongoDatabase = _mongoClientFactory.GetClient(_configuration["MongoDbConfig:ConnectionString"]!).GetDatabase(_configuration["MongoDbConfig:DatabaseName"]);
            return mongoDatabase.GetCollection<StudentModel>("StudentDetails");
        }

        public async Task CreateNewStudent(StudentModel student)
        {
            await GetCollection().InsertOneAsync(student);
            return;
        }

        public async Task<bool> DeleteAllStudent()
        {
            var isDeleted = await GetCollection().DeleteManyAsync(x => true);

            return isDeleted.IsAcknowledged && isDeleted.DeletedCount > 0;
        }

        public async Task<bool> DeleteStudent(string username)
        {
            var filter = Builders<StudentModel>.Filter.Eq("Username", username);
            var deleteResult = await GetCollection().DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<StudentModel> GetStudent(string username)
        {
            return await GetCollection().Find(student => student.Username == username).SingleOrDefaultAsync();
        }

        public async Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            int skipStudent = (pageNumber - 1) * itemPerPage;
            return await GetCollection().Find(student => (university == "all" || student.University == university) && (department == "all" || student.Department == department))
            .Skip(skipStudent)
            .Limit(itemPerPage)
            .ToListAsync();
        }

        public async Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument)
        {
            var filter = Builders<StudentModel>.Filter.Eq("Username", username);

            var findStudent = await GetCollection().Find(filter).FirstOrDefaultAsync();

            if (findStudent == null)
            {
                return false;
            }

            patchDocument.ApplyTo(findStudent);


            var result = await GetCollection().ReplaceOneAsync(filter, findStudent);

            return result.ModifiedCount > 0;
        }

        public async Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            return await GetCollection().Find(student => (university == "all" || student.University == university) && (department == "all" || student.Department == department)).CountDocumentsAsync();
        }

        public async Task<List<UserInfoUpdateEvent>> UpdateStudent(string username, UpdateStudentModel student)
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

            var previousStudent = await GetCollection().Find(currentInfo).FirstOrDefaultAsync();

            var updateResult = await GetCollection().UpdateOneAsync(currentInfo, updateInfo);

            var updateEvents = new List<UserInfoUpdateEvent>();

            if(updateResult.ModifiedCount > 0)
            {
                
                foreach (var property in typeof(UpdateStudentModel).GetProperties())
                {
                    var propertyName = property.Name;
                    var previousValue = previousStudent.GetType().GetProperty(propertyName)?.GetValue(previousStudent, null)?.ToString();
                    var currentValue = property.GetValue(student)?.ToString();

                    if (previousValue != currentValue)
                    {
                        updateEvents.Add(new UserInfoUpdateEvent
                        {
                            UserName = username,
                            UserInfoField = propertyName,
                            PreviousUserInfoFieldValue = previousValue,
                            CurrentUserInfoFieldValue = currentValue
                        });
                    }
                }
            }

            return updateEvents;
        }
    }
}
