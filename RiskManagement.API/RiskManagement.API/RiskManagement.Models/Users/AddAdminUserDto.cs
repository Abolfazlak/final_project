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
    
    [Required]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "Mobile number must start with 09 and be 11 digits long.")]
    public string PhoneNumber { get; set; }
}
public class AddAdminUserDto : AddUserDto
{
    [Required]
    public string CompanyName { get; set; }
}