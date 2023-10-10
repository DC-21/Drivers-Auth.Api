using DriverAuth.Api.Configurations;
using DriverAuth.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DriverAuth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthManagementController: ControllerBase
{
    private readonly ILogger<AuthManagementController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AuthManagementController(ILogger<AuthManagementController> logger, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig>optionsMonitor)
    {
        _logger = logger;
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
    {
        if (ModelState.IsValid)
        {
            //check if email exists in database
            var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);
            if (emailExist != null)
                return BadRequest("Email already exists");
            //ceate new user now
            var newUser = new IdentityUser()
            {
                Email = requestDto.Email
            };
            var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
            if (isCreated.Succeeded)
            {
                //generate token
                return Ok(new RegisterRequestResponse()
                {
                    Result = true,
                    Token = ""
                });
            }

            return BadRequest("error creating the user, please try again later");
        }

        return BadRequest("Invalid request payload");
    }
}