using Microsoft.Extensions.Configuration;
using Contracts.MongoClientFactory;
using MongoDB.Driver;
using NotificationApi.Business.Notification;
using NotificationApi.Contracts.Models;
using Contracts;

namespace NotificationApi.Repository.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MongoClientFactory _mongoClientFactory;
        private readonly IConfiguration _configuration;

        public NotificationRepository(MongoClientFactory mongoClientFactory, IConfiguration configuration)
        {
            _mongoClientFactory = mongoClientFactory;
            _configuration = configuration;
        }

        private IMongoCollection<UserNotification> GetCollection()
        {
            var mongoDatabase = _mongoClientFactory.GetClient(_configuration["MongoDbConfig:ConnectionString"]!).GetDatabase(_configuration["MongoDbConfig:DatabaseName"]);
            return mongoDatabase.GetCollection<UserNotification>("NotificationDetails");
        }
        public async Task<List<UserNotification>> GetNotifications(int pageNumber, int pageSize)
        {
            int skipNotification = (pageNumber - 1) * pageSize;
            return await GetCollection().Find(x => true)
                .Skip(skipNotification)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task CreateNotification(UserNotification userNotification)
        {
            await GetCollection().InsertOneAsync(userNotification);
            return;
        }
    }
}
