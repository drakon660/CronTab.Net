using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Crontab.Identity.Controllers;

[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private const string TokenSecret = "KissMyAssMotherFucker";
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromSeconds(30);

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        var token = JwtCreator.GenerateToken(request.Email, TokenSecret, TokenLifeTime);
        Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true});
        Response.Cookies.Append("X-Username", request.Email, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None ,Secure = true });
        Response.Cookies.Append("X-Refresh-Token", GenerateRefreshToken(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
        
        return Ok(token);
    }
    
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
        if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
            return BadRequest();

        // var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName && i.RefreshToken == refreshToken);
        //
        // if (user == null)
        //     return BadRequest();
        
       
        var token = JwtCreator.GenerateToken(userName, TokenSecret, TokenLifeTime);
        
        Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true});
        Response.Cookies.Append("X-Username", userName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None ,Secure = true });
        Response.Cookies.Append("X-Refresh-Token", GenerateRefreshToken(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });

        return Ok();    
    }    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
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
            //new("userid", request.UserId.ToString()),
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