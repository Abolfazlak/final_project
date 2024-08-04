namespace RiskManagement.API.RiskManagement.DataProvide;

public class Company
{
    public long Id { get; set; }
    public string CompanyName { get; set; }

    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Project> Projects { get; set; }

}