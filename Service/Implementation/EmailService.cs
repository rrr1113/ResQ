using Domain.Configuration;
using Domain.Dto.Email;
using Microsoft.Extensions.Options;
using MimeKit;
using Service.Interface;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Service.Implementation;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }
    
    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct = default)
    {
        var email = new MimeMessage();
        
        email.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;
        
        var builder = new BodyBuilder()
        {
            HtmlBody = message.HtmlBody,
            TextBody = message.PlainText
        };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, 
                _settings.UseSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.Auto, ct);
            
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
        }
        finally
        {
            smtp.DisconnectAsync(true, ct);
        }
    }
}