using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Domain.Accounts;
using Hoje.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Hoje.Exceptions.Accounts;


namespace Hoje.Api.Features.Accounts.Repository.Implementation;

public class AccountRepository : IAccountRepository
{
    private readonly HojeDbContext _context;
    private readonly IUserService _userService;

    public AccountRepository(HojeDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    private async Task<Guid> GetAuthenticatedUserIdAsync()
    {
        Guid? authenticatedUserId = await _userService.GetAuthenticatedUserIdAsync();
        if (authenticatedUserId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");
        return authenticatedUserId.Value;
    }

    public async Task<Account> CreateAccount(Account account)
    {
        if (account.UserId != GetAuthenticatedUserIdAsync().Result)
            throw new UnauthorizedAccessException("User is not authorized to create this account.");
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> GetAccountById(Guid id)
    {
        return await _context.Accounts.FirstOrDefaultAsync(account => account.Id == id && account.UserId == GetAuthenticatedUserIdAsync().Result);
    }

    public async Task<List<Account>> GetAccountsByUserId(Guid userId)
    {
        return await _context.Accounts.Where(account => account.UserId == userId && !account.IsArchived && account.UserId == GetAuthenticatedUserIdAsync().Result).ToListAsync();
    }

    public async Task<Account> UpdateAccount(Account account)
    {
        if (account.UserId != GetAuthenticatedUserIdAsync().Result)
            throw new ResourceNotFoundException();
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task DeleteAccountAsync(Guid accountId)
    {
        Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == GetAuthenticatedUserIdAsync().Result);
        if (account == null)
            throw new ResourceNotFoundException();
        account.IsArchived = true; // Soft delete
        account.ArchivedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

}
