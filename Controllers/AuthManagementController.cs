using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DriverAuth.Api.Configurations;
using DriverAuth.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            //create new user now
            var newUser = new IdentityUser()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email
            };
            var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
            if (isCreated.Succeeded)
            {
                var token = GenerateJwtToken(newUser);
                //generate token
                return Ok(new RegisterRequestResponse()
                {
                    Result = true,
                    Token = token
                });
            }

            return BadRequest(isCreated.Errors.Select(x=>x.Description).ToList());
        }

        return BadRequest("Invalid request payload");
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512)
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }
}