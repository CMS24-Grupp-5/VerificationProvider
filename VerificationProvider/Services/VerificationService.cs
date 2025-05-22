using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VerificationProvider.Data;
using VerificationProvider.Entities;
using VerificationProvider.Models;

namespace VerificationProvider.Services;

public class VerificationService(DataContext context)
{
    private readonly DataContext _context = context;
    public async Task<SendEmailModel?> CreateVerificationEmail(string email)
    {
        
        var randomCode = GenerateVerificationCode();
        var emailMessage = MapVerificationEmail(email, randomCode);
        var saveCodeResult = await SaveCodeInDatabaseAsync(email, randomCode);
        
        if (saveCodeResult == false)
        {
            return null;
        }
        
        return emailMessage;
    }

    public async Task<bool> ValidateCodeAsync(ValidateCodeRequest request)
    {
        try
        {
            var compareCodeResult = await CompareWithDatabaseAsync(request.Code);

            if (compareCodeResult == false)
            {
                return false;
            }
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public async Task<bool> DeleteExpiredCodesAsync()
    {
        var result = _context.Set<VerifyEmailEntity>().Where(x => x.ExpiryTime < DateTime.UtcNow).ToList();

        if (!result.IsNullOrEmpty())
        {
            _context.RemoveRange(result);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    
    private SendEmailModel MapVerificationEmail(string email, string code)
    {
        List<string> emailList = [email];
        
        var emailMessage = new SendEmailModel
        {
            Recipients = emailList,
            Subject = $"Ventixe - Verify your email",
            PlainText = $"Your email verification code is: {code}",
            Html = $"<html><body><h1><div>Your email verification code is:</div><div>{code}</div></h1><h2><div>The code is valid for 5 minutes.</div></h2></body></html>"
        };
        return emailMessage;
    }
    
    private string GenerateVerificationCode()
    {
        var randomNumber = new Random().Next(100000, 999999);
        return randomNumber.ToString();
    }

    private async Task<bool> SaveCodeInDatabaseAsync(string email, string randomCode, int minutes = 5)
    {
        try
        {
            var saveCode = new VerifyEmailEntity
            {
                Code = randomCode,
                ExpiryTime = DateTime.UtcNow.AddMinutes(minutes)
            };

            _context.Add(saveCode);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    private async Task<bool> CompareWithDatabaseAsync(string code)
    {
        try
        {
            var result = await _context.Set<VerifyEmailEntity>().FirstOrDefaultAsync(x =>
                x.Code == code &&
                x.ExpiryTime > DateTime.UtcNow
            );

            if (result == null)
            {
                return false;
            }

            _context.Remove(result);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    
}