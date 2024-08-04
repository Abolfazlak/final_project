using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;

namespace RiskManagement.API.RiskManagement.Helpers;

public partial class CommonHelper(IConfiguration configuration) : ICommonHelper
{
    public JwtSecurityToken GenerateAccessToken(long userId, bool isAdmin, long companyId)
    {
        // Create user claims
        var claims = new List<Claim>
        {
            new("userId", userId.ToString()),
            new("companyId", companyId.ToString()),
            new("isAdmin", isAdmin.ToString())
        };

        // Create a JWT
        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7), // Token expiration time
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
    public string GetHash(string password)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
    
    public bool ValidateMobileNumber(string mobileNumber)
    {
        var regex = MobileRegex();
        return regex.IsMatch(mobileNumber);
    }
    public bool ValidatePassword(string password)
    {
        var regex = PasswordRegex();
        return regex.IsMatch(password);
    }
    
    [GeneratedRegex(@"^09\d{9}$")]
    private static partial Regex MobileRegex();
    [GeneratedRegex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")]
    private static partial Regex PasswordRegex();
}