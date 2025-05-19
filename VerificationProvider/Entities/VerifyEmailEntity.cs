using System.ComponentModel.DataAnnotations;

namespace VerificationProvider.Entities;

public class VerifyEmailEntity
{
    [Key] public string Id { get; set; } = null!;
    
    public string Code { get; set; } = null!;
}