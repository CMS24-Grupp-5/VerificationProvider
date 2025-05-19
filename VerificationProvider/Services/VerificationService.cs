using VerificationProvider.Models;

namespace VerificationProvider.Services;

public class VerificationService
{

    private readonly List<string> _emailList = [];

    public SendEmailModel CreateVerificationEmail(string email)
    {
        _emailList.Add(email);
        
        var randomCode = GenerateVerificationCode();
        var emailMessage = MapVerificationEmail(email, randomCode);

        return emailMessage;

    }
    
    
    private SendEmailModel MapVerificationEmail(string email, string code)
    {
        var emailMessage = new SendEmailModel
        {
            Recipients = _emailList,
            Subject = $"Ventixe - Verify your email",
            PlainText = $"Your email verification code is: {code}",
            Html = $"<html><body><h1><div>Your email verification code is:</div><div>{code}</div></h1></body></html>"
        };
        return emailMessage;
    }
    
    private string GenerateVerificationCode()
    {
        var randomNumber = new Random().Next(100000, 999999);
        return randomNumber.ToString();
    }
}