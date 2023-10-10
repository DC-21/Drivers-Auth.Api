using Microsoft.AspNetCore.Mvc;

namespace DriverAuth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthManagementController: ControllerBase
{
    private readonly ILogger<AuthManagementController> _logger;

    public AuthManagementController(ILogger<AuthManagementController> logger)
    {
        _logger = logger;
    }
}