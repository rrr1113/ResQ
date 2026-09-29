using System.Threading.Channels;
using Domain.Dto.Email;
using Service.Interface;

namespace Service.Implementation;

public class EmailQueue : IEmailQueue
{
    private readonly Channel<EmailMessage> _channel;

    public EmailQueue(Channel<EmailMessage> channel)
    {
        _channel = channel;
    }
    
    public async Task EnqueueAsync(EmailMessage message)
    {
        await _channel.Writer.WriteAsync(message);
    }
}