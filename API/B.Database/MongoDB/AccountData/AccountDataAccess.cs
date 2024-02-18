
using A.Contracts.DTOs;
using A.Contracts.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace B.Database.MongoDB.AccountData
{
    public class AccountDataAccess : IAccountDataAccess
    {
        private readonly IMongoCollection<AccountModel> _mongoCollection;

        public AccountDataAccess(IConfiguration configuration)
        {
            // Configure MongoDB
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
            _mongoCollection = mongoDatabase.GetCollection<AccountModel>("AccountDetails");
        }


        public async Task<bool> UserExists(string username)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Username", username);

            var findUser = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            if (findUser == null)
            {
                return false;
            }
            return true;
        }

        public async Task Register(AccountModel accountModel)
        {
            await _mongoCollection.InsertOneAsync(accountModel);
        }

        public async Task<AccountModel> Login(LoginModel loginModel)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Username", loginModel.Username);

            var user = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> DeleteAccount(string username)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Username", username);
            var deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }
    }
}
