using Microsoft.AspNetCore.Mvc;

namespace PolePosition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "PolePosition API radi!" });
    }
}