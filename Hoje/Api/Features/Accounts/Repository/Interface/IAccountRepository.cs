
using Hoje.Domain.Accounts;

public interface IAccountRepository
{
    Task<Account> CreateAccount(Account account);
    Task<Account?> GetAccountById(Guid id);
    Task<List<Account>> GetAccountsByUserId(Guid userId);
    Task<Account> UpdateAccount(Account account);
    Task DeleteAccountAsync(Guid accountId);
}