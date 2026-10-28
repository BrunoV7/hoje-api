
using Hoje.Domain.Accounts;
using Hoje.Domain.Accounts.DTO;
using Hoje.Model.Accounts;

namespace Hoje.Api.Features.Accounts.Service.Interface;

public interface IAccountService
{
    Task<AccountResponse> CreateAccountAsync(CreateAccountRequest dto);
    Task<AccountResponse?> GetAccountByIdAsync(Guid id);
    Task<Account?> InternalGetAccountByIdAsync(Guid id);
    Task<List<AccountResponse>> GetAccountsByUserIdAsync();
    Task<AccountResponse> UpdateAccountAsync(AccountUpdateRequest account, Guid accountId);
    Task DeleteAccountAsync(Guid accountId);
}