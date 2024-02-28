﻿
using A.Contracts.DTOs;
using A.Contracts.Models;

namespace C.Business.Messages
{
    public interface IMessageRepository
    {
        Task CreateMessage(MessageDto message);
        Task<bool> DeleteMessage(string id);
        Task<List<Message>> GetMessages(string sender, string receiver, int pageNumber, int itemPerPage);
        Task<List<MessageListDto>> GetList(string username, int pageNumber, int itemPerPage);
        Task<long> GetNumberOfList(string username);
        Task<long> GetNumberOfMessage(string sender, string receiver);
    }
}