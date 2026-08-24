using api.silisync.Extensions;
using application.silisync.Interfaces.Application;
using domain.silisync.Requests.Users;
using domain.silisync.Responses;
using Microsoft.AspNetCore.Mvc;

namespace api.silisync.Controllers.Auth;

[ApiController]
[Route("v1/[controller]")]
public class Auth(IAuthApi authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors.Select(error => error.ErrorMessage))
                .ToList();

            var response = new Response<object>(
                code: StatusCodes.Status400BadRequest,
                message: "Invalid field(s)",
                errors: errors);
            
            return StatusCode(response.Code, response);
        }
        
        var result = await authService.RegisterAsync(request);
        return result.ToActionResult();
    }
}