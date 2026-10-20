using Hoje.Api.Features.Auth.DTO;
using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.Service.Interface;

public interface IUserService
{
    Task<MeResponse?> GetAuthenticatedUserAsync();
}