using A.Contracts.Update_Models;
using Business;
using Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Business.Admins
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClientFactory _mongoClientFactory;

        public AdminRepository(IConfiguration configuration, MongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
            _configuration = configuration;
        }

        private IMongoCollection<AdminModel> GetCollection()
        {
            var mongoDatabase = _mongoClientFactory.GetClient(_configuration["MongoDbConfig:ConnectionString"]!).GetDatabase(_configuration["MongoDbConfig:DatabaseName"]);
            return mongoDatabase.GetCollection<AdminModel>("AdminDetails");
        }

        public async Task CreateNewAdmin(AdminModel admin)
        {
            await GetCollection().InsertOneAsync(admin);
            return;
        }

        public async Task<bool> DeleteAdmin(string username)
        {
            var filter = Builders<AdminModel>.Filter.Eq("Username", username);
            var deleteResult = await GetCollection().DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAllAdmin()
        {
            var isDeleted = await GetCollection().DeleteManyAsync(x => true);

            return isDeleted.IsAcknowledged && isDeleted.DeletedCount > 0;
        }

        public async Task<AdminModel> GetAdmin(string username)
        {
            return await GetCollection().Find(admin => admin.Username == username).SingleOrDefaultAsync();
        }

        public async Task<List<AdminModel>> GetAdmins(int pageNumber, int itemPerPage)
        {
            int skipAdmin = (pageNumber - 1) * itemPerPage;
            return await GetCollection().Find(admin => true)
            .Skip(skipAdmin)
            .Limit(itemPerPage)
            .ToListAsync();
        }

        public async Task<bool> PartiallyUpdateAdmin(string username, JsonPatchDocument<AdminModel> patchDocument)
        {
            var filter = Builders<AdminModel>.Filter.Eq("Username", username);

            var findAdmin = await GetCollection().Find(filter).FirstOrDefaultAsync();

            if (findAdmin == null)
            {
                return false;
            }

            patchDocument.ApplyTo(findAdmin);


            var result = await GetCollection().ReplaceOneAsync(filter, findAdmin);

            return result.ModifiedCount > 0;
        }

        public async Task<long> TotalNumberOfAdmins(int pageNumber, int itemPerPage)
        {
            return await GetCollection().Find(admin => true).CountDocumentsAsync();
        }

        public async Task<bool> UpdateAdmin(string username, UpdateAdminModel admin)
        {
            var currentInfo = Builders<AdminModel>.Filter.Eq("Username", username);

            var updateInfo = Builders<AdminModel>.Update
                .Set(s => s.Name, admin.Name)
                .Set(s => s.Gender, admin.Gender)
                .Set(s => s.Blood_group, admin.Blood_group);

            var updateResult = await GetCollection().UpdateOneAsync(currentInfo, updateInfo);

            return updateResult.ModifiedCount > 0;
        }
    }
}
