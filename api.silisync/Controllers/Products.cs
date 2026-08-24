using application.silisync.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;

namespace api.silisync.Controllers;

[ApiController]
[Route("v1/[controller]")]
public sealed class Products : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(IProductApplication productApplication, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productApplication.GetAllAsync(cancellationToken);

            return Ok(products);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(IProductApplication productApplication, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productApplication.GetByIdAsync(id, cancellationToken);

            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}