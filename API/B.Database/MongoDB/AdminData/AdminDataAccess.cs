

using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace B.Database.MongoDB.AdminData
{
    public class AdminDataAccess : IAdminDataAccess
    {
        private readonly IMongoCollection<AdminModel> _mongoCollection;

        public AdminDataAccess(IConfiguration configuration)
        {
            // Configure MongoDB
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
            _mongoCollection = mongoDatabase.GetCollection<AdminModel>("AdminDetails");
        }

        public async Task CreateNewAdmin(AdminModel admin)
        {
            await _mongoCollection.InsertOneAsync(admin);
            return;
        }

        public async Task<bool> DeleteAdmin(string username)
        {
            var filter = Builders<AdminModel>.Filter.Eq("Username", username);
            var deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAllAdmin()
        {
            var isDeleted = await _mongoCollection.DeleteManyAsync(x => true);

            return isDeleted.IsAcknowledged && isDeleted.DeletedCount > 0;
        }

        public async Task<AdminModel> GetAdmin(string username)
        {
            return await _mongoCollection.Find(admin => admin.Username == username).SingleOrDefaultAsync();
        }

        public async Task<List<AdminModel>> GetAdmins(int pageNumber, int itemPerPage)
        {
            int skipAdmin = (pageNumber - 1) * itemPerPage;
            return await _mongoCollection.Find(admin => true)
            .Skip(skipAdmin)
            .Limit(itemPerPage)
            .ToListAsync();
        }

        public async Task<bool> PartiallyUpdateAdmin(string username, JsonPatchDocument<AdminModel> patchDocument)
        {
            var filter = Builders<AdminModel>.Filter.Eq("Username", username);

            var findAdmin = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            if (findAdmin == null)
            {
                return false;
            }

            patchDocument.ApplyTo(findAdmin);


            var result = await _mongoCollection.ReplaceOneAsync(filter, findAdmin);

            return result.ModifiedCount > 0;
        }

        public async Task<long> TotalNumberOfAdmins(int pageNumber, int itemPerPage)
        {
            return await _mongoCollection.Find(admin => true).CountDocumentsAsync();
        }

        public async Task<bool> UpdateAdmin(string username, UpdateAdminModel admin)
        {
            var currentInfo = Builders<AdminModel>.Filter.Eq("Username", username);

            var updateInfo = Builders<AdminModel>.Update
                .Set(s => s.Name, admin.Name)
                .Set(s => s.Gender, admin.Gender)
                .Set(s => s.Blood_group, admin.Blood_group);

            var updateResult = await _mongoCollection.UpdateOneAsync(currentInfo, updateInfo);

            return updateResult.ModifiedCount > 0;
        }
    }
}
