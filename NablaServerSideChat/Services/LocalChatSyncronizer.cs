using NablaServerSideChat.Entities;

namespace NablaServerSideChat.Services;

public sealed class LocalChatSyncronizer : IDisposable
{
    public int ChatId { get; private set; }
    public IGlobalChatSyncronizer Parent { get; private set; }
    public event Action<ChatMessage> OnMessageReceived = null!;

    public LocalChatSyncronizer(IGlobalChatSyncronizer parent, int chatId)
    {
        ChatId = chatId;
        Parent = parent;
    }

    public void Send(ChatMessage message)
    {
        using var scope = Parent.ServiceProvider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<IChatStorage>();
        storage.Add(message);
        if (OnMessageReceived == null)
        {
            return;
        }
        OnMessageReceived.Invoke(message);
    }

    public void NotifyClientUnlinked()
    {
        int invocationCount = OnMessageReceived?.GetInvocationList().Length ?? 0;
        if (invocationCount == 0)
        {
            Parent.UnsyncChat(ChatId);
        }
    }

    public void Dispose()
    {
        if (OnMessageReceived == null)
        {
            return;
        }
        foreach (var del in OnMessageReceived.GetInvocationList())
        {
            OnMessageReceived -= (Action<ChatMessage>)del;
        }
    }
}