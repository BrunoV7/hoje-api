using Hoje.Domain.Accounts;
using Hoje.Domain.Transactions;
using Hoje.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Hoje.Infrastructure.Data;

public class HojeDbContext : DbContext
{    
    public  HojeDbContext(DbContextOptions<HojeDbContext> options) : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Account>().HasKey(a => a.Id);
        modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Account>()
            .HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Account>(account =>
        {
            account.ComplexProperty(a => a.OpeningBalance, money =>
            {
                money.Property(m => m.Cents).HasColumnName("OpeningBalance").IsRequired();
                money.Property(m => m.Currency).HasColumnName("OpeningBalanceCurrency").IsRequired().HasMaxLength(3);
            });
        });
        modelBuilder.Entity<Transaction>(amount =>
        {
            amount.ComplexProperty(a => a.Amount, money =>
            {
                money.Property(m => m.Cents).HasColumnName("TransactionAmount").IsRequired();
                money.Property(m => m.Currency).HasColumnName("TransactionCurrency").IsRequired().HasMaxLength(3);
            });
        });
    }
    
}