using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationProvider.Services;

namespace VerificationProvider.Actions;

public class VerificationCodeTrigger(ILogger<VerificationCodeTrigger> logger, VerificationService verificationService)
{
    private readonly ILogger<VerificationCodeTrigger> _logger = logger;
    private readonly VerificationService _verificationService = verificationService;

    [Function(nameof(VerificationCodeTrigger))]
    [ServiceBusOutput("emailservice", Connection = "ServiceBus")]
    public async Task<string?> Run(
        [ServiceBusTrigger("verificationcode", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        var email = message.Body.ToString();
        
        if (!String.IsNullOrEmpty(email))
        {
            var emailMessage = _verificationService.CreateVerificationEmail(email);
            await messageActions.CompleteMessageAsync(message);

            return JsonSerializer.Serialize(emailMessage);
        }
        
        _logger.LogWarning("No email was received.");
        await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object>{{"Reason", "No email address was received."}});
        return null;
    }
}