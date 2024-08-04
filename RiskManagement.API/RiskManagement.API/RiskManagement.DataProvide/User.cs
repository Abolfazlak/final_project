using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class User
{
    [Key]
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }
    public long CompanyId { get; set; }
    public bool IsActive { get; set; }
    public virtual Company Company { get; set; }
}