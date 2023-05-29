using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;
using Shared.Dto.Args;
using Shared.Services;

namespace ApiGateway.Controllers;

[Controller]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController : ControllerBase
{
    private readonly IAuthGrpcService _authGrpcService;

    public AuthController(IAuthGrpcService authGrpcService)
    {
        _authGrpcService = authGrpcService;
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserArgs args)
    {
        await _authGrpcService.CreateUser(args);

        return Ok();
    }

    [HttpPost("remove")]
    public async Task<ActionResult> RemoveUser([FromBody] string email)
    {
        await _authGrpcService.RemoveUser(email);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(string email, string password)
    {
        var response = await _authGrpcService.GetAccessToken(new GetAccessTokenArgs
        {
            Email = email,
            Password = password
        });

        return new TokenDto
        {
            AccessToken = response.AccessToken
        };
    }

    [HttpGet("authorize_player")]
    public ActionResult<string> AuthorizePlayer()
    {
        return User.Identity.Name;
    }
}