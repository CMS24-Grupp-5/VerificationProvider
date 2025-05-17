using VerificationProvider.Models;

namespace VerificationProvider.Services;

public class VerificationService
{

    private readonly List<string> _emailList = [];
    
    public SendEmailModel CreateVerificationEmail(string email)
    {
        _emailList.Add(email);
        var randomCode = GenerateVerificationCode();
        
        var emailMessage = new SendEmailModel
        {
            Recipients = _emailList,
            Subject = $"Here is your verification code",
            PlainText = $"Your verification code is {randomCode}",
            Html = $"<html><body><h1><div>Your verification code is:</div><div>{randomCode}</div></h1></body></html>"
        };
        return emailMessage;
    }
    
    private static string GenerateVerificationCode()
    {
        var randomNumber = new Random().Next(100000, 999999);
        return randomNumber.ToString();
    }
}