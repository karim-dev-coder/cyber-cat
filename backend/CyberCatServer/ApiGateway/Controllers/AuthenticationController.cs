using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[Controller]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    public const string TOKEN = "test_token";

    /// <summary>
    /// Выдача токена по email и паролю.
    /// </summary>
    /// <returns>Токен</returns>
    [HttpGet]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
    public IActionResult Login(string email, string password)
    {
        return Ok(TOKEN);
    }
}