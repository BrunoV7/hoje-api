using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.DTO;

public class CurrentUserResponse(User user)
{
    public Guid  UserId { get; set; } = user.Id;
    public string Email { get; set; } = user.Email;
    public string FullName { get; set; } = user.FirstName + " " + user.LastName;
}
    