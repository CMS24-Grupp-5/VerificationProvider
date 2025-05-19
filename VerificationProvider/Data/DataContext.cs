using Microsoft.EntityFrameworkCore;

namespace VerificationProvider.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    
}