using System.ComponentModel.DataAnnotations;

namespace VerificationProvider.Entities;

public class VerifyEmailEntity
{
    [Key] [MaxLength(150)] public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [MaxLength(6)]
    public string Code { get; set; } = null!;
    
    public DateTime ExpiryTime { get; set; }
}