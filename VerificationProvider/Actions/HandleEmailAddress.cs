using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace VerificationProvider.Actions;

public class HandleEmailAddress
{
    private readonly ILogger<HandleEmailAddress> _logger;
    
    public HandleEmailAddress(ILogger<HandleEmailAddress> logger)
    {
        _logger = logger;
    }

    [Function("HandleEmailAddress")]
    [ServiceBusOutput("verificationcode", Connection = "ServiceBus")]
    public async Task<string> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        return requestBody;
    }

}