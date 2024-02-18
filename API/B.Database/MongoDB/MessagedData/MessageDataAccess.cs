
using A.Contracts.DTOs;
using A.Contracts.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace B.Database.MongoDB.MessagedData
{
    public class MessageDataAccess : IMessageDataAccess
    {
        private readonly IMongoCollection<Message> _mongoCollection;

        public MessageDataAccess(IConfiguration configuration)
        {
            // Configure MongoDB
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
            _mongoCollection = mongoDatabase.GetCollection<Message>("MessageDetails");
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
            await _mongoCollection.InsertOneAsync(content);
            return;
        }

        public async Task<bool> DeleteMessage(string id)
        {
            return true;
        }

        public async Task<List<MessageListDto>> GetList(string username, int pageNumber, int itemPerPage)
        {
            int skipList = (pageNumber - 1) * itemPerPage;

            var uniqueMessages = await _mongoCollection
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
            var uniqueMessages = await _mongoCollection
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
            var messages = await _mongoCollection.Find(msg => (msg.Sender == sender && msg.Receiver == receiver) || (msg.Sender == receiver && msg.Receiver == sender))
                .SortByDescending(msg => msg.Created)
                .Skip(skipMessage)
                .Limit(itemPerPage)
                .ToListAsync();

            messages.Reverse();

            return messages;
        }

        public async Task<long> GetNumberOfMessage(string sender, string receiver)
        {
            return await _mongoCollection.Find(msg => (msg.Sender == sender && msg.Receiver == receiver) || (msg.Sender == receiver && msg.Receiver == sender))
                .CountDocumentsAsync();
        }
    }
}
