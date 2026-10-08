using Microsoft.AspNetCore.Mvc;

namespace MvcAuthApp.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
}
