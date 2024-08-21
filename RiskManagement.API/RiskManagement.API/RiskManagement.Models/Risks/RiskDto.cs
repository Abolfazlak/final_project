using RiskManagement.API.RiskManagement.DataProvide;

namespace RiskManagement.API.RiskManagement.Models.Risks;

public class RiskDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Methodology { get; set; }
    public MainRiskCategoryDto MainRiskCategory { get; set; }
    public SecondaryRiskCategoryDto SecondaryRiskCategory { get; set; }
}

public class RiskStatusDto : RiskDto
{
    public int Status { get; set; }
    public DateTime EstimatedFinishedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public long EstimatedAmount { get; set; }
    public long? FinalAmount { get; set; }
    public UpdateSolutionDto? BestSolution { get; set; }
    public int? Score { get; set; }
    public bool? IsOpportunity { get; set; }

}

public class InputRiskStatusDto
{
    public long Id { get; set; }
    public int Status { get; set; }
    public DateTime FinishedDate { get; set; }
    public long FinalAmount { get; set; }
    public long? SolutionId { get; set; }
    public string? SolutionTitle { get; set; }
    public bool IsNewSolution { get; set; }

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