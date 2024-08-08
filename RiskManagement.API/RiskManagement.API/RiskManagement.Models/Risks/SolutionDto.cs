namespace RiskManagement.API.RiskManagement.Models.Risks;

public class InputSolutionDto
{
    public string Description { get; set; }
    public long Amount { get; set; }
    public long RiskId { get; set; }
}

public class UpdateSolutionDto : InputSolutionDto
{
    public long Id { get; set; }

}