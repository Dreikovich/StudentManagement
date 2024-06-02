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
            var userUuid = _loginRepository.GetStudentUuid(loginDto.Login);
            if (userUuid == null) return Unauthorized();
            loginDto.StudentUuid = userUuid;
            var isValid = _loginRepository.ValidateLogin(loginDto);
            if (!isValid) return Unauthorized();

            var role = _loginRepository.GetUserRole(loginDto);

            var tokenString = GenerateToken(loginDto, role);
            return Ok(new { Token = tokenString, Role = role });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    private string GenerateToken(LoginDto loginDto, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("superSecretKey@345:)superSecretKey@345:)");
        var expires = DateTime.UtcNow.AddHours(24);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, loginDto.StudentUuid),
                new(ClaimTypes.Name, loginDto.Login),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, loginDto.StudentUuid)
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