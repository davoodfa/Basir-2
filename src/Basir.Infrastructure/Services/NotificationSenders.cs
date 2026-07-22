using Basir.Application.Auth.Interfaces;
using Microsoft.Extensions.Logging;

namespace Basir.Infrastructure.Services;

public class EmailSender : IEmailService
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendConfirmationAsync(string email, string link, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] Confirmation to {Email}: {Link}", email, link);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string email, string link, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] Password reset to {Email}: {Link}", email, link);
        return Task.CompletedTask;
    }

    public Task SendTwoFactorCodeAsync(string email, string code, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] 2FA code to {Email}: {Code}", email, code);
        return Task.CompletedTask;
    }
}

public class SmsSender : ISmsService
{
    private readonly ILogger<SmsSender> _logger;

    public SmsSender(ILogger<SmsSender> logger)
    {
        _logger = logger;
    }

    public Task SendTwoFactorCodeAsync(string phoneNumber, string code, CancellationToken ct = default)
    {
        _logger.LogInformation("[SMS] 2FA code to {PhoneNumber}: {Code}", phoneNumber, code);
        return Task.CompletedTask;
    }
}
