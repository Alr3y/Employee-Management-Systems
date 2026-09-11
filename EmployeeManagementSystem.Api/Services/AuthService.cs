using EmployeeManagementSystem.Api.Data;
using EmployeeManagementSystem.Api.DTOs.Auth;
using EmployeeManagementSystem.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AuthResponseDto?> AuthenticateAsync(string username, string password)
    {
        username = username.Trim();
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user is null) return null;

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verify == PasswordVerificationResult.Failed) return null;

        var jwt = _configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var expiresMinutes = int.TryParse(jwt["ExpiresMinutes"], out var m) ? m : 60;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };
        if (!string.IsNullOrEmpty(user.Role))
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

        var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return new AuthResponseDto(token, expires);
    }

    public void SeedAdminUser()
    {
        var hasher = new PasswordHasher<User>();
        var user = new User { Username = "admin", Role = "Admin" };
        user.PasswordHash = hasher.HashPassword(user, "password123");

        _context.Users.Add(user);
        _context.SaveChanges();
    }
}