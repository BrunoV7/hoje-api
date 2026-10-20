using Hoje.Api.Features.Auth.DTO;
using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.Service.Security.Interface;

public interface IJwtService
{
    public string GenerateToken(User user);
    public Task<User?> GetUserFromToken(string token);
}