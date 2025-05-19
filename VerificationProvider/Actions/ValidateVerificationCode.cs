using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VerificationProvider.Models;

namespace VerificationProvider.Actions;

public class ValidateVerificationCode
{
    private readonly ILogger<ValidateVerificationCode> _logger;

    public ValidateVerificationCode(ILogger<ValidateVerificationCode> logger)
    {
        _logger = logger;
    }

    [Function("ValidateVerificationCode")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var body = new StreamReader(req.Body).ReadToEnd();
        var verificationRequest = JsonConvert.DeserializeObject<ValidateCodeRequest>(body);
        
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }

}