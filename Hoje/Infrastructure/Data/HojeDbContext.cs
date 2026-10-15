using Microsoft.EntityFrameworkCore;

namespace Hoje.Infrastructure.Data;

public class HojeDbContext : DbContext
{
    public  HojeDbContext(DbContextOptions<HojeDbContext> options) : base(options)
    {}
    
    
}