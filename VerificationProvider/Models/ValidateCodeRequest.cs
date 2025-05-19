namespace VerificationProvider.Models;

public class ValidateCodeRequest
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } =null!;
}