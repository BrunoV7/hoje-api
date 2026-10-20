using Hoje.Api.Features.Auth.DTO;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Hoje.Api.Features.Auth.DTO.LoginRequest;

namespace Hoje.Api.Features.Auth.Service.Interface;

public interface IAuthService
{
    public Task<TokenResponse> Login(LoginRequest request);
    public Task<RegisterResponse> Register(RegistrationRequest request);
}