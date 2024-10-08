using System.Collections.Concurrent;

namespace NablaServerSideChat.Services;

public interface IGlobalChatSyncronizer : IDisposable
{
    IServiceProvider ServiceProvider { get; }
    LocalChatSyncronizer GetChatSyncronizer(int chatId);
    void UnsyncChat(int id);
}

public sealed class GlobalChatSyncronizer : IGlobalChatSyncronizer
{
    public IServiceProvider ServiceProvider { get; private set; }
    private ConcurrentDictionary<int, LocalChatSyncronizer> _dict = [];

    public GlobalChatSyncronizer(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public LocalChatSyncronizer GetChatSyncronizer(int chatId)
        => _dict.GetOrAdd(chatId, key => new LocalChatSyncronizer(this, key));

    public void UnsyncChat(int id)
    {
        if (!_dict.Remove(id, out var chat))
        {
            return;
        }
        chat.Dispose();
    }

    public void Dispose()
    {
        foreach (var pair in _dict)
        {
            pair.Value.Dispose();
        }
        _dict.Clear();
    }
}
