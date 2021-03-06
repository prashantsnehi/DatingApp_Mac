using AutoMapper.QueryableExtensions;

namespace API.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public MessageRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddGroup(Group group)
    {
        _context.Groups.Add(group);
    }

    public void RemoveConnection(Connection connection)
    {
        _context.Connections.Remove(connection);
    }

    public async Task<Connection> GetConnection(string connectionId) =>
            await _context.Connections.FindAsync(connectionId);
    public async Task<Group> GetMessageGroup(string groupName) =>
            await _context.Groups
                    .Include(x => x.Connections)
                    .FirstOrDefaultAsync(x => x.Name == groupName);
    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessage(int id) =>
        await _context.Messages
                        .Include(u => u.Sender)
                        .Include(u => u.Recipient)
                        .SingleOrDefaultAsync(x => x.Id == id);
    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParam messageParam)
    {
        var query = _context.Messages
                    .OrderByDescending(m => m.MessageSent)
                    .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

        query = messageParam.Container switch
        {
            "Inbox" => query.Where(u => u.RecipientUsername == messageParam.Username && u.RecipientDeleted == false),
            "Outbox" => query.Where(u => u.SenderUsername == messageParam.Username && u.SenderDeleted == false),
            _ => query.Where(u => u.RecipientUsername == messageParam.Username
                               && u.RecipientDeleted == false && u.DateRead == null)
        };

        return await PagedList<MessageDto>.CreateAsync(query, messageParam.PageNumber, messageParam.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await _context.Messages
            .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                    && m.Sender.UserName == recipientUsername
                    || m.Recipient.UserName == recipientUsername
                    && m.Sender.UserName == currentUsername && m.SenderDeleted == false
            )
            .OrderBy(m => m.MessageSent)
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var unreadMessages = messages.Where(m => m.DateRead == null
            && m.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }
        }

        return messages;
    }

    // public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;

    public async Task<Group> GetGroupForConnection(string connectionId) =>
            await _context.Groups
                        .Include(c => c.Connections)
                        .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                        .FirstOrDefaultAsync();
}
