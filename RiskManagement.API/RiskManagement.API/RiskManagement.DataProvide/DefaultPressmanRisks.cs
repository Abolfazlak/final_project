using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class DefaultPressmanRisks
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int SecondaryRiskCategoryId { get; set; }

    public virtual SecondaryRiskCategory SecondaryRiskCategory{ get; set; }
}