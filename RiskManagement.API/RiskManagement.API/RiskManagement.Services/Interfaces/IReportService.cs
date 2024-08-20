using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Risks;

namespace RiskManagement.API.RiskManagement.Services.Interfaces;

public interface IReportService
{
    public Task<ResponseMessage<int[,]>> GetRiskMatrixById(long id);
    public Task<ResponseMessage<int[,]>> GetOpportunityMatrixById(long id);
    public Task<ResponseMessage<Dictionary<int, int>>> GetRiskCountsByStatus(long id);
    public Task<ResponseMessage<Dictionary<string, int>?>> GetRiskCountsByRupPhases(long id);
    public Task<ResponseMessage<List<RiskAmountSummary>>> GetRiskAmountSummariesAsync(long id);


}