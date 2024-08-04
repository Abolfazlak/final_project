using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class SecondaryRiskCategory
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int MainRiskCategoryId { get; set; }

    public virtual MainRiskCategory MainRiskCategory{ get; set; }
}