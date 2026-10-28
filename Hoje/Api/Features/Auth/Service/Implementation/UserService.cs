using System.Security.Claims;
using Hoje.Api.Features.Auth.DTO;
using Hoje.Api.Features.Auth.Repository.Interface;
using Hoje.Api.Features.Auth.Service.Interface;
using Hoje.Api.Features.Auth.Service.Security.Interface;
using Hoje.Domain.Users;
using Hoje.Exceptions.Auth;

namespace Hoje.Api.Features.Auth.Service;

public class UserService(
    IAuthRepository userRepository,
    IHttpContextAccessor contextAccessor) : IUserService
{
    public async Task<CurrentUserResponse?> GetAuthenticatedUserAsync()
    {
        var principal = contextAccessor.HttpContext?.User;

        var userId = principal?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var id))
            throw new UnauthenticatedException();

        User? user = await userRepository.GetUserById(id);

        if (user is null)
            throw new UnauthorizedAccessException();

        return new CurrentUserResponse(user);
    }
    public async Task<User?> GetAuthenticatedUserAsyncInternal()
    {
        var principal = contextAccessor.HttpContext?.User;

        var userId = principal?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var id))
            throw new UnauthenticatedException();

        User? user = await userRepository.GetUserById(id);

        if (user is null)
            throw new UnauthorizedAccessException();

        return user;
    }

    public async Task<Guid?> GetAuthenticatedUserIdAsync()
    {
        var principal = contextAccessor.HttpContext?.User;

        var userId = principal?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var id))
            throw new UnauthenticatedException();

        User? user = await userRepository.GetUserById(id);

        if (user is null)
            throw new UnauthorizedAccessException();

        return user.Id;
    }
}