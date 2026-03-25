using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected string UserId => User.FindFirstValue("id") ?? string.Empty;
}
