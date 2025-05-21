using Microsoft.EntityFrameworkCore;
using VerificationProvider.Entities;

namespace VerificationProvider.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    private DbSet<VerifyEmailEntity> EmailVerification { get; set; }
}