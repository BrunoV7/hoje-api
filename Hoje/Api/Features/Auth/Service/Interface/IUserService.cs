using Hoje.Api.Features.Auth.DTO;
using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.Service.Interface;

public interface IUserService
{
    Task<CurrentUserResponse?> GetAuthenticatedUserAsync();
    Task<User?> GetAuthenticatedUserAsyncInternal();
    Task<Guid?> GetAuthenticatedUserIdAsync();
}