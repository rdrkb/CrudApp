﻿

using A.Contracts.DTOs;
using A.Contracts.Models;
using B.Database.MongoDB.MessagedData;

namespace C.Business.MessageLogic
{
    public class MessageLogic : IMessageLogic
    {
        private readonly IMessageDataAccess _messageDataAccess;

        public MessageLogic(IMessageDataAccess messageDataAccess)
        {
            _messageDataAccess = messageDataAccess;
        }

        public async Task CreateMessage(MessageDto message)
        {
            await _messageDataAccess.CreateMessage(message);
            return;
        }

        public async Task<bool> DeleteMessage(string id)
        {
            return await _messageDataAccess.DeleteMessage(id);
        }

        public async Task<List<MessageListDto>> GetList(string username, int pageNumber, int itemPerPage)
        {
            return await _messageDataAccess.GetList(username, pageNumber, itemPerPage);
        }

        public async Task<List<Message>> GetMessages(string sender, string receiver, int pageNumber, int itemPerPage)
        {
            List<Message> messages = await _messageDataAccess.GetMessages(sender, receiver, pageNumber, itemPerPage);
            return messages;
        }

        public async Task<long> GetNumberOfList(string username)
        {
            return await _messageDataAccess.GetNumberOfList(username);
        }

        public async Task<long> GetNumberOfMessage(string sender, string receiver)
        {
            return await _messageDataAccess.GetNumberOfMessage(sender, receiver);
        }
    }
}
