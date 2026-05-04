namespace Mymarket.Application.Interfaces;

public interface IChatNotifier
{
    Task SendMessage(string chatId, string message);
}
