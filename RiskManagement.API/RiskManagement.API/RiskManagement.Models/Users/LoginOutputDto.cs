namespace RiskManagement.API.RiskManagement.Models.Users;

public class LoginOutputDto
{
    public string Token { get; set; }
    public bool IsAdmin { get; set; }
}