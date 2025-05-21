using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationProvider.Services;

namespace VerificationProvider.Actions;

public class RemoveExpiredCodes
{
    private readonly ILogger<RemoveExpiredCodes> _logger;
    private readonly VerificationService _verificationService;

    public RemoveExpiredCodes(ILogger<RemoveExpiredCodes> logger, VerificationService verificationService)
    {
        _logger = logger;
        _verificationService = verificationService;
    }

    [Function("RemoveExpiredCodes")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        var result = await _verificationService.DeleteExpiredCodesAsync();

        if (result)
        {
            _logger.LogInformation($"Expired codes removed at: {DateTime.UtcNow}");
        }
    }
}