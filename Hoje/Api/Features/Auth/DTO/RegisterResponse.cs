using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.DTO;

public class RegisterResponse(User user, string accessToken, DateTime expiresAt)
{
    public Guid  UserId { get; set; } = user.Id;
    public string Email { get; set; } = user.Username;
    public string FullName { get; set; } = user.Fullname;
    public string AccessToken {  get; set; } = accessToken;
    public DateTime ExpiresAt { get; set; } = expiresAt;
}