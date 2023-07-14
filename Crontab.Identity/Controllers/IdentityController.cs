using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Crontab.Identity.Controllers;

[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private const string TokenSecret = "KissMyAssMotherFucker";
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromMinutes(1);

    [HttpPost]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.UserId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = "https://localhost:7269",
            Audience = "https://localhost:7159",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);
    }
    [HttpPost("admin")]
    public IActionResult GenerateTokenForAdmin([FromBody] TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.UserId.ToString()),
            new(IdentityData.AdminUserClaimName, "true")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = "https://localhost:7269",
            Audience = "https://localhost:7159",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);
    }
}