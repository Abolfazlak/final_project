using RiskManagement.API.RiskManagement.Models;

namespace RiskManagement.API.RiskManagement.Services.Interfaces;

public interface IReportService
{
    public Task<ResponseMessage<int[,]>> GetRiskMatrixById(long id);
    public Task<ResponseMessage<int[,]>> GetOpportunityMatrixById(long id);
    public Task<ResponseMessage<Dictionary<int, int>>> GetRiskCountsByStatus(long id);
    public Task<ResponseMessage<Dictionary<string, int>?>> GetRiskCountsByRupPhases(long id);

}