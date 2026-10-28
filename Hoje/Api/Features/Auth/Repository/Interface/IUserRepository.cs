
using Hoje.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<bool> DoesUserExistWithEmail(string email);
    Task<bool> DoesUserExistWithIdAsync(Guid id);

}