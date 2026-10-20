namespace Hoje.Api.Features.Auth.DTO;

public record TokenResponse(string AccessToken, DateTime ExpiresAt);