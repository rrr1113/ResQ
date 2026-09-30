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
            smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
            
            await smtp.ConnectAsync(
                _settings.SmtpHost,
                _settings.SmtpPort,
                MailKit.Security.SecureSocketOptions.StartTls,
                ct);
            
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password, ct);
            await smtp.SendAsync(email);
        }
        finally
        {
            if (smtp.IsConnected)
            {
                await smtp.DisconnectAsync(true, ct);
            }
        }
    }
}