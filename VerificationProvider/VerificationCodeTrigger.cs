using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VerificationProvider;

public class VerificationCodeTrigger
{
    private readonly ILogger<VerificationCodeTrigger> _logger;

    public VerificationCodeTrigger(ILogger<VerificationCodeTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(VerificationCodeTrigger))]
    public async Task Run(
        [ServiceBusTrigger("verificationcode", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
      
        await messageActions.CompleteMessageAsync(message);
        
    }
}