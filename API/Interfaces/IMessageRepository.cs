using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParam messageParam);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientusername);
        Task<bool> SaveAllAsync();
    }
}