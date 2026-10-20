using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.DTO;

public class MeResponse(User user)
{
    public Guid  UserId { get; set; } = user.Id;
    public string Email { get; set; } = user.Username;
    public string FullName { get; set; } = user.Fullname;
}
