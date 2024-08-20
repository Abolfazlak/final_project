namespace RiskManagement.API.RiskManagement.Models.Risks;

public class RiskAmountSummary
{
    public int Status { get; set; }
    public long SumOfFinalAmount { get; set; }
    public long SumOfEstimatedAmount { get; set; }
}