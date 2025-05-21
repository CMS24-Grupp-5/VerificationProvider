using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VerificationProvider.Models;
using VerificationProvider.Services;

namespace VerificationProvider.Actions;

public class ValidateVerificationCode
{
    private readonly ILogger<ValidateVerificationCode> _logger;
    private readonly VerificationService _verificationService;

    public ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, VerificationService verificationService)
    {
        _logger = logger;
        _verificationService = verificationService;
    }

    [Function("ValidateVerificationCode")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var verificationRequest = JsonConvert.DeserializeObject<ValidateCodeRequest>(body);

        if (verificationRequest == null)
        {
            _logger.LogError("Some error occured with the post.");
            return new BadRequestResult();
        }
        
        var result = await _verificationService.ValidateCodeAsync(verificationRequest);

        return result
            ? new OkObjectResult(new { message = "Code is accurate" })
            : new UnauthorizedObjectResult(new { message = "Code is invalid or expired" });
    }

}