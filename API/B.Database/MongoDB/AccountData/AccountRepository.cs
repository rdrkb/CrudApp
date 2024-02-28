
using A.Contracts.DTOs;
using A.Contracts.Models;
using C.Business.Accounts;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace B.Database.MongoDB.AccountData
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClientFactory _mongoClientFactory;

        public AccountRepository(IConfiguration configuration, MongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
            _configuration = configuration;
        }

        private IMongoCollection<AccountModel> GetCollection()
        {
            var mongoDatabase = _mongoClientFactory.GetClient(_configuration["MongoDbConfig:ConnectionString"]!).GetDatabase(_configuration["MongoDbConfig:DatabaseName"]);
            return mongoDatabase.GetCollection<AccountModel>("AccountDetails");
        }

        public async Task<bool> UserExists(string username)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Username", username);

            var findUser = await GetCollection().Find(filter).FirstOrDefaultAsync();

            if (findUser == null)
            {
                return false;
            }
            return true;
        }

        public async Task Register(AccountModel accountModel)
        {
            await GetCollection().InsertOneAsync(accountModel);
        }

        public async Task<AccountModel> Login(LoginModel loginModel)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Username", loginModel.Username);

            var user = await GetCollection().Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> DeleteAccount(string username)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Username", username);
            var deleteResult = await GetCollection().DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }
    }
}
