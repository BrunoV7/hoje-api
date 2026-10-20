using Hoje.Api.Features.Auth.DTO;
using Hoje.Api.Features.Auth.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Hoje.Api.Features.Auth.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    //TODO: Implement Global Exception Handler
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegistrationRequest request)
    {
        return Ok(await _authService.Register(request));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        return Ok(await _authService.Login(request));
    }
    
}