using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.Repository.Interface;

public interface IAuthRepository
{
    Task<User?> GetUserByEmail(string email);
    Task<bool> DoesUserExistWithEmail(string email);
    Task<User> CreateUser(User newUser);
    Task<User?> GetUserById(Guid id);
}