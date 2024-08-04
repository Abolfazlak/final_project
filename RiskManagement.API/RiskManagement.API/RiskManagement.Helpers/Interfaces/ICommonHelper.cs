using System.IdentityModel.Tokens.Jwt;

namespace RiskManagement.API.RiskManagement.Helpers.Interfaces;

public interface ICommonHelper
{
    public string GetHash(string password);
    public JwtSecurityToken GenerateAccessToken(long userId, bool isAdmin, long companyId);
    public bool ValidateMobileNumber(string mobileNumber);
    public bool ValidatePassword(string password);
}