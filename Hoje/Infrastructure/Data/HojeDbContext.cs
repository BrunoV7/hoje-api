using Hoje.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Hoje.Infrastructure.Data;

public class HojeDbContext : DbContext
{
    public  HojeDbContext(DbContextOptions<HojeDbContext> options) : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    
}