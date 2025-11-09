using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Mymarket.Application.Interfaces;

namespace Mymarket.Infrastructure.Services;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    private readonly string smtpServer = configuration.GetValue<string>("SmtpSettings:SmtpServer")!;
    private readonly int smtpPort = configuration.GetValue<int>("SmtpSettings:SmtpPort")!;
    private readonly string smtpUsername = configuration.GetValue<string>("SmtpSettings:SmtpUsername")!;
    private readonly string smtpPassword = configuration.GetValue<string>("SmtpSettings:SmtpPassword")!;

    public void SendEmail(
        string SenderName,
        string SenderEmail,
        string ToName,
        string ToEmail,
        string Subject,
        string TextContent)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(SenderName, SenderEmail));
        message.To.Add(new MailboxAddress(ToName, ToEmail));
        message.Subject = Subject;
        message.Body = new TextPart("plain")
        {
            Text = TextContent
        };

        using var client = new SmtpClient();

        client.Connect(smtpServer, smtpPort, false);
        client.Authenticate(smtpUsername, smtpPassword);

        try
        {
            client.Send(message);
            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}
