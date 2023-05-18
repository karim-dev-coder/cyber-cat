using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;
using Shared.Services;

namespace ApiGateway.Controllers;

[Controller]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SolutionController : ControllerBase
{
    private readonly ISolutionGrpcService _solutionService;

    public SolutionController(ISolutionGrpcService solutionService)
    {
        _solutionService = solutionService;
    }

    [HttpGet("{taskId}")]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    public async Task<ActionResult<string>> GetLastSavedCode(string taskId)
    {
        var userId = User.Identity.GetUserId();
        var args = new GetSavedCodeArgs
        {
            UserId = userId,
            TaskId = taskId
        };

        var savedCode = await _solutionService.GetSavedCode(args);
        return savedCode.SolutionCode;
    }

    [HttpPost("{taskId}")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult> SaveCode(string taskId, [FromBody] string sourceCode)
    {
        if (string.IsNullOrEmpty(sourceCode))
        {
            throw new ArgumentNullException(nameof(sourceCode));
        }

        var userId = User.Identity.GetUserId();
        var args = new SolutionArgs
        {
            UserId = userId,
            TaskId = taskId,
            SolutionCode = sourceCode
        };

        await _solutionService.SaveCode(args);

        return Ok();
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    public async Task<ActionResult<string>> DeleteSavedCode(string taskId)
    {
        var userId = User.Identity.GetUserId();
        var args = new RemoveCodeArgs
        {
            UserId = userId,
            TaskId = taskId
        };

        await _solutionService.RemoveCode(args);

        return Ok();
    }
}