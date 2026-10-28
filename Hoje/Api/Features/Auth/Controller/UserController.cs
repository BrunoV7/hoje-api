using Hoje.Api.Features.Auth.DTO;
using Hoje.Api.Features.Auth.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoje.Api.Features.Auth.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> GetUser()
    {
        var user = await userService.GetAuthenticatedUserAsync();

        return Ok(user);
    }
}