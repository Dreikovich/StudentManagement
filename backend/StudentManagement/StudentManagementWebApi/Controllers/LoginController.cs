using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]

[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginRepository _loginRepository;
    
    public LoginController(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }
    
    [HttpPost("Addlogin")]
    public IActionResult AddLogin([FromBody] LoginDto loginDto)
    {
        try
        {
            _loginRepository.AddLogin(loginDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpPost("VerifyLogin")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var isValid = _loginRepository.ValidateLogin(loginDto);
            if (!isValid)
            {
                return Unauthorized();
            }

            var tokenString = GenerateToken(loginDto);
            return Ok(new { Token = tokenString });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    private string GenerateToken(LoginDto loginDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("superSecretKey@345:)");
        var expires = DateTime.UtcNow.AddHours(1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, loginDto.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = expires,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        _loginRepository.SaveTokenToDatabase(tokenString, expires, loginDto.StudentUuid);
        return tokenString;
    }
}