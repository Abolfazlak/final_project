using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class MainRiskCategory
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    
    public virtual ICollection<SecondaryRiskCategory> SecondaryRiskCategories { get; set; }
}