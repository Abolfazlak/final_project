using Microsoft.EntityFrameworkCore;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;

namespace RiskManagement.API.RiskManagement.Repositories;

public class ReportRepo(RiskManagementDbContext context) : IReportRepo
{
    public async Task<List<RiskAmountSummary>> GetRiskAmountSummariesAsync(long id)
    {
        // Fetch the initial grouped data
        var riskAmountSummaries = await context.Risks
            .Where(r => r.ProjectId == id)
            .GroupBy(r => r.Status)
            .Select(g => new RiskAmountSummary
            {
                Status = g.Key,
                SumOfFinalAmount = g.Sum(r => r.FinalAmount ?? 0),
                SumOfEstimatedAmount = g.Sum(r => r.RiskDetails.Sum(rd => rd.EstimatedRiskAmount) 
                + r.RiskDetails.Sum(rd => rd.EstimatedOpportunityAmount))
            })
            .ToListAsync();

        // Ensure statuses 0, 1, and 2 are included with zero amounts if missing
        var requiredStatuses = new[] { 0, 1, 2 };

        foreach (var status in requiredStatuses)
        {
            if (!riskAmountSummaries.Any(r => r.Status == status))
            {
                riskAmountSummaries.Add(new RiskAmountSummary
                {
                    Status = status,
                    SumOfFinalAmount = 0,
                    SumOfEstimatedAmount = 0
                });
            }
        }

        return riskAmountSummaries;
    }

    
    public async Task<int[,]> GetRiskCountsGrid(long id)
    {
        var riskGrid = new int[5, 5]; // 5x5 grid initialized to 0

        var riskDetails = context.Risks.Include(r => r.RiskDetails)
            .Where(r => r.ProjectId == id)
            .SelectMany(r => r.RiskDetails)
            .Where(rd => !rd.IsOpportunity 
                         && rd.RiskProbability.HasValue 
                         && rd.RiskImpact.HasValue);

        foreach (var detail in riskDetails)
        {
            if (detail.RiskProbability == null) continue;
            var probability = detail.RiskProbability.Value - 1; // Convert to 0-based index
            if (detail.RiskImpact == null) continue;
            var impact = detail.RiskImpact.Value - 1; // Convert to 0-based index
            riskGrid[probability, impact]++;
        }

        return await Task.FromResult(riskGrid);
    }
    
    public async Task<int[,]> GetOpportunityCountsGrid(long id)
    {
        var opportunityGrid = new int[3, 3]; // 3x3 grid initialized to 0

        var opportunityDetails = context.Risks.Include(r => r.RiskDetails)
            .Where(r => r.ProjectId == id)
            .SelectMany(r => r.RiskDetails)
            .Where(rd => rd.IsOpportunity 
                         && rd.OpportunityProbability.HasValue 
                         && rd.OpportunityImpact.HasValue);

        foreach (var detail in opportunityDetails)
        {
            if (detail.OpportunityProbability == null) continue;
            var probability = detail.OpportunityProbability.Value - 1; // Convert to 0-based index
            if (detail.OpportunityImpact == null) continue;
            var impact = detail.OpportunityImpact.Value - 1; // Convert to 0-based index
            opportunityGrid[probability, impact]++;
        }

        return await Task.FromResult(opportunityGrid);
    }
    
    
    public async Task<Dictionary<string, int>?> GetRiskCountsByRupPhase(long projectId)
    {
        // Get the project with the specified projectId
        var project = context.Projects.
            Include(project => project.Risks).
            ThenInclude(risk => risk.RiskDetails).
            SingleOrDefault(p => p.Id == projectId && p.Methodology == "rup");
        
        if (project == null)
        {
            return await Task.FromResult<Dictionary<string, int>?>(null);
        }

        // Initialize a dictionary with all RUP phases set to 0
        var rupPhaseCounts = RupPhases.ToDictionary(phase => phase, phase => 0);

        // Flatten RiskDetails and count risks by RUP phase
        var phaseRiskCounts = project.Risks
            .SelectMany(r => r.RiskDetails)
            .Where(rd => rd.RupPhase != null)
            .GroupBy(rd => rd.RupPhase)
            .ToDictionary(group => group.Key, group => group.Count());

        // Update the dictionary with the actual counts
        foreach (var phase in phaseRiskCounts)
        {
            if (phase.Key != null) rupPhaseCounts[phase.Key] = phase.Value;
        }

        return rupPhaseCounts;
    }
    
    public async Task<Dictionary<int, int>> GetRiskCountsByStatus(long id)
    {
        // Initialize a dictionary with all statuses set to 0
        var statusCounts = new Dictionary<int, int>
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 0 }
        };

        // Group risks by Status and count them
        var groupedStatusCounts = context.Risks
            .Where(r => r.ProjectId == id)
            .GroupBy(r => r.Status)
            .ToDictionary(group => group.Key, group => group.Count());

        // Update the dictionary with actual counts
        foreach (var status in groupedStatusCounts)
        {
            statusCounts[status.Key] = status.Value;
        }

        return await Task.FromResult(statusCounts);
    }

    private static readonly List<string> RupPhases = ["Inception", "Elaboration", "Construction", "Transition"];


    

}