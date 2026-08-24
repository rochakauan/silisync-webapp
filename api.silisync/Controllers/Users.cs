using api.silisync.Extensions;
using application.silisync.Interfaces.Application;
using domain.silisync.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.silisync.Controllers;

[ApiController]
[Route("v1/users")]
public sealed class Users(IUserApplication userApplication) : ControllerBase
{
    
    [HttpGet]
    public async ValueTask<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
       var result = await userApplication.GetAllUsersAsync(cancellationToken);
       return result.ToActionResult();
    }
    
    [HttpPost]
    public async ValueTask<IActionResult> GetByEmail(
        [FromQuery] string name,
        [FromServices] IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var response = await repository.
            GetUserByEmail(name, cancellationToken);

        return !response.IsSuccess ? 
            StatusCode(404, response.Error)
            : Ok(response);
    }
}