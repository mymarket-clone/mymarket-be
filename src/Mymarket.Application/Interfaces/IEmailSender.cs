using Microsoft.Extensions.Configuration;

namespace Mymarket.Application.Interfaces;

public interface IEmailSender
{
    void SendEmail(
        string SenderName,
        string SenderEmail,
        string ToName,
        string ToEmail,
        string Subject,
        string TextContent);
}
