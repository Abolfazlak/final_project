using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;

namespace RiskManagement.API.RiskManagement.Helpers;

public static class TokenHelper
{
    public static string? GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
        return usernameClaim?.Value;
    }
    
    public static string? GetUserIsAdminFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "isAdmin");
        return usernameClaim?.Value;
    }
    
    public static string? GetUCompanyIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "companyId");
        return usernameClaim?.Value;
    }
}