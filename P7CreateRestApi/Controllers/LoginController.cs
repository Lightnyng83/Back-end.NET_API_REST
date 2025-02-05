using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<IdentityUser> _passwordHasher;

    public LoginController(UserManager<IdentityUser> userManager, IConfiguration configuration, IPasswordHasher<IdentityUser> passwordHasher)
    {
        _userManager = userManager;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        // Vérifie le mot de passe
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        if (result == PasswordVerificationResult.Success)
        {
            var token = GenerateJwtToken(user);
            return Ok(new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(1)
            });
        }

        return Unauthorized("Invalid username or password.");
    }


    private string GenerateJwtToken(IdentityUser user)
    {
        // Vérifier que l'utilisateur est valide
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new Exception("UserName est null ou vide.");
        }

        if (string.IsNullOrEmpty(user.Id))
        {
            throw new Exception("User Id est null ou vide.");
        }

        // Vérifier que la configuration JWT est présente
        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("La clé JWT n'est pas configurée.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Ajout des claims pour l'utilisateur
        var claims = new[]
  {
    new Claim(ClaimTypes.Name, user.UserName), // <span style="color: green;">Ajoute ce claim pour renseigner Name</span>
    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim(ClaimTypes.Role, "User")
};


        // Création du token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}


