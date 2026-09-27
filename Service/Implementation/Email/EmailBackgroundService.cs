using System.Threading.Channels;
using Domain.Dto.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Interface;

namespace Service.Implementation;

public class EmailBackgroundService : BackgroundService
{
    private readonly Channel<EmailMessage> _channel;
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailBackgroundService(Channel<EmailMessage> channel, IServiceScopeFactory scopeFactory)
    {
        _channel = channel;
        _scopeFactory = scopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider
                    .GetRequiredService<IEmailService>();
                await emailService.SendEmailAsync(message, stoppingToken);
            }
            catch (Exception ex)
            {
            } 
        }
    }
}