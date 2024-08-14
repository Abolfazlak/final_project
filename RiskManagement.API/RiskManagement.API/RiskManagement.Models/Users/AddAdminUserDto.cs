using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.Models.Users;


public class AddUserDto
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password must be at least 8 characters long, contain at least one letter, one number, and one special character.")]
    public string Password { get; set; }
    
    [Required]
    public string FullName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
}
public class AddAdminUserDto : AddUserDto
{
    [Required]
    public string CompanyName { get; set; }
}