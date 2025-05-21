using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationProvider.Services;

namespace VerificationProvider.Actions;

public class SendVerificationCode(ILogger<SendVerificationCode> logger, VerificationService verificationService)
{
    private readonly ILogger<SendVerificationCode> _logger = logger;
    private readonly VerificationService _verificationService = verificationService;

    [Function(nameof(SendVerificationCode))]
    [ServiceBusOutput("emailservice", Connection = "ServiceBus")]
    public async Task<string?> Run(
        [ServiceBusTrigger("verificationcode", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        var email = message.Body.ToString();
        
        if (!String.IsNullOrEmpty(email))
        {
            var emailMessage = await _verificationService.CreateVerificationEmail(email);
            
            if (emailMessage == null)
            {
                await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object> { { "Reason", "There was an error with the database." } });
            }
            
            await messageActions.CompleteMessageAsync(message);

            return JsonSerializer.Serialize(emailMessage);
        }
        
        _logger.LogWarning("No email was received.");
        await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object>{{"Reason", "No email address was received."}});
        return null;
    }
}