using Hoje.Domain.Users;

namespace Hoje.Api.Features.Auth.DTO;

public class RegisterResponse(User user, string accessToken, DateTime expiresAt)
{
    public Guid  UserId { get; set; } = user.Id;
    public string Email { get; set; } = user.Email;
    public string FullName { get; set; } = user.FirstName + " " + user.LastName;
    public string AccessToken {  get; set; } = accessToken;
    public DateTime ExpiresAt { get; set; } = expiresAt;
}