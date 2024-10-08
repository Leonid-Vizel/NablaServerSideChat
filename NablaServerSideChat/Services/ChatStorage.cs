using System.Collections.Concurrent;
using NablaServerSideChat.Entities;

namespace NablaServerSideChat.Services;

public interface IChatStorage
{
    List<ChatMessage> GetMessages(int chatId);
    void Add(ChatMessage message);
}

public sealed class ChatStorage : IChatStorage
{
    public static ConcurrentBag<ChatMessage> Messages { get; set; } = [];

    public List<ChatMessage> GetMessages(int chatId)
        => Messages.Where(x => x.ChatId == chatId)
            .OrderBy(x => x.Time)
            .ToList();

    public void Add(ChatMessage message)
    {
        var latestId = Messages.Where(x => x.ChatId == message.ChatId)
            .Select(x => x.Id)
            .OrderByDescending(x => x)
            .FirstOrDefault();
        message.Id = latestId + 1;
        Messages.Add(message);
    }
}
