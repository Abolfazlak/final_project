using RiskManagement.API.RiskManagement.DataProvide;

namespace RiskManagement.API.RiskManagement.Models.Risks;

public class RiskDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public MainRiskCategoryDto MainRiskCategory { get; set; }
    public SecondaryRiskCategoryDto SecondaryRiskCategory { get; set; }
}

public class MainRiskCategoryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public class SecondaryRiskCategoryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
}