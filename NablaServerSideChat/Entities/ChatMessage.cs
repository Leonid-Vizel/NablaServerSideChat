namespace NablaServerSideChat.Entities;

public sealed class ChatMessage
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = null!;
    public ChatMessage(int chatId, int userId, string message)
    {
        Time = DateTime.Now;
        ChatId = chatId;
        UserId = userId;
        Message = message;
    }
}
