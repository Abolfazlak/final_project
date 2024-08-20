using RiskManagement.API.RiskManagement.Models.Risks;

namespace RiskManagement.API.RiskManagement.Repositories.Interfaces;

public interface IReportRepo
{
    public Task<int[,]> GetRiskCountsGrid(long id);
    public Task<int[,]> GetOpportunityCountsGrid(long id);
    public Task<Dictionary<string, int>?> GetRiskCountsByRupPhase(long projectId);
    public Task<Dictionary<int, int>> GetRiskCountsByStatus(long id);
    public Task<List<RiskAmountSummary>> GetRiskAmountSummariesAsync(long id);


}