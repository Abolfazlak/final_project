namespace RiskManagement.API.RiskManagement.Models.Risks;

public class RiskInputDto
{
    public string Title { get; set; }
    public SecondaryRiskCategoryDto? SecondaryRiskCategory { get; set; }
    public string? SecondaryRiskCategoryTitle { get; set; }
    public long ProjectId { get; set; }
}

public class RiskUpdateDto : RiskInputDto
{
    public long Id { get; set; }
}