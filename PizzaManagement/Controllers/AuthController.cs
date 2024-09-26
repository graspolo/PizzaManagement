using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [SwaggerRequestBody(Description = "Esempio di credenziali di login", Required = true, Example = typeof(UserLoginExample))]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
        // Verifichiamo le credenziali dell'utente (per semplicità, usiamo credenziali hardcoded)
        if (userLogin.Username == "admin" && userLogin.Password == "admin")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("simone-mulas-bit-secret-key-256-bits");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", "1") }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        return Unauthorized();
    }
}

public class UserLogin
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class UserLoginExample
{
    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "admin";
}

public class SwaggerRequestBodyAttribute : Attribute
{
    public string Description { get; set; }
    public bool Required { get; set; }
    public Type Example { get; set; }
}
