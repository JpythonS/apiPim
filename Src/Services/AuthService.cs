using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using api_pim.Interfaces;
using api_pim.Exceptions;
using System.Net;

namespace api_pim.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly string _secretKey;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _secretKey = configuration["Jwt:SecretKey"]!;
    }

    public string Authenticate(string email, string senha)
    {
        try
        {
            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                return null!;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "apiPim",
                audience: "frontendPim",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }
        catch (Exception)
        {
            throw new ApiException((int)HttpStatusCode.InternalServerError, $"Erro interno [{ErrorCode.AS}]");
        }
    }
}