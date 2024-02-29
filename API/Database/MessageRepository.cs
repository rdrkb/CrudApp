using Business;
using Business.Messages;
using Contracts.DTOs;
using Contracts.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClientFactory _mongoClientFactory;

        public MessageRepository(IConfiguration configuration, MongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
            _configuration = configuration;
        }

        private IMongoCollection<Message> GetCollection()
        {
            var mongoDatabase = _mongoClientFactory.GetClient(_configuration["MongoDbConfig:ConnectionString"]!).GetDatabase(_configuration["MongoDbConfig:DatabaseName"]);
            return mongoDatabase.GetCollection<Message>("MessageDetails");
        }

        public async Task CreateMessage(MessageDto message)
        {
            var content = new Message
            {
                Id = "",
                Sender = message.Sender,
                Receiver = message.Receiver,
                Content = message.Content,
                Created = DateTime.UtcNow,
                IsRead = false
            };
            await GetCollection().InsertOneAsync(content);
            return;
        }

        public async Task<bool> DeleteMessage(string id)
        {
            return true;
        }

        public async Task<List<MessageListDto>> GetList(string username, int pageNumber, int itemPerPage)
        {
            int skipList = (pageNumber - 1) * itemPerPage;

            var uniqueMessages = await GetCollection()
                .Find(msg => msg.Sender == username || msg.Receiver == username)
                .SortByDescending(msg => msg.Created)
                .ToListAsync();

            var groupedMessages = uniqueMessages
                .GroupBy(msg => msg.Sender == username ? msg.Receiver : msg.Sender)
                .Select(group => group.First())
                .Skip(skipList)
                .Take(itemPerPage)
                .ToList();

            var messageList = groupedMessages.Select(msg => new MessageListDto
            {
                Sender = msg.Sender,
                Receiver = msg.Receiver,
                LastMessage = msg.Created
            }).ToList();

            return messageList;
        }
        public async Task<long> GetNumberOfList(string username)
        {
            var uniqueMessages = await GetCollection()
                .Find(msg => msg.Sender == username || msg.Receiver == username)
                .SortByDescending(msg => msg.Created)
                .ToListAsync();

            return uniqueMessages
                .GroupBy(msg => msg.Sender == username ? msg.Receiver : msg.Sender)
                .Select(group => group.First())
                .Count();
        }

        public async Task<List<Message>> GetMessages(string sender, string receiver, int pageNumber, int itemPerPage)
        {
            int skipMessage = (pageNumber - 1) * itemPerPage;
            var messages = await GetCollection().Find(msg => msg.Sender == sender && msg.Receiver == receiver || msg.Sender == receiver && msg.Receiver == sender)
                .SortByDescending(msg => msg.Created)
                .Skip(skipMessage)
                .Limit(itemPerPage)
                .ToListAsync();

            messages.Reverse();

            return messages;
        }

        public async Task<long> GetNumberOfMessage(string sender, string receiver)
        {
            return await GetCollection().Find(msg => msg.Sender == sender && msg.Receiver == receiver || msg.Sender == receiver && msg.Receiver == sender)
                .CountDocumentsAsync();
        }
    }
}
