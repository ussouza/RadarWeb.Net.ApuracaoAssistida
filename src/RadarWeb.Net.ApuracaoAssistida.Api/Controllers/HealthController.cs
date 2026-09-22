using Microsoft.AspNetCore.Mvc;

namespace RadarWeb.Net.ApuracaoAssistida.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        status = "ok",
        service = "RadarWeb.Net.ApuracaoAssistida.Api",
        timestamp = DateTimeOffset.UtcNow
    });
}
