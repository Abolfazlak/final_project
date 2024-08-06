namespace RiskManagement.API.RiskManagement.Models.Risks;

public class RiskDetailsDto
{
    public string? Description { get; set; }
    public string? RupPhase { get; set; }
    public bool IsOpportunity { get; set; } // if false then is Risk

    public int? RiskProbability { get; set; }
    public int? RiskImpact { get; set; }
    public int? RiskScore { get; set; }
    public int? OpportunityProbability { get; set; }
    public int? OpportunityImpact { get; set; }
    public int? OpportunityScore { get; set; }
    
    public long? EstimatedRiskAmount { get; set; }
    public long? EstimatedOpportunityAmount { get; set; }
    
    public string? Document { get; set; }
    public DateTime EstimatedDateTime { get; set; }
    
    public long RiskId { get; set; }
}


public class RiskDetailUpdateDto : RiskDetailsDto
{
    public long Id { get; set; }
}