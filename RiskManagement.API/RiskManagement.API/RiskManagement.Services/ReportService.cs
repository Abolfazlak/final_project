using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services.Interfaces;
using Exception = System.Exception;

namespace RiskManagement.API.RiskManagement.Services;

public class ReportService(IReportRepo repo) : IReportService
{
    public async Task<ResponseMessage<int[,]>> GetRiskMatrixById(long id)
    {
        try
        {
            var matrix = await repo.GetRiskCountsGrid(id);

            return new ResponseMessage<int[,]>
            {
                Code = 200,
                Content = matrix
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<int[,]>
            {
                Code = 500,
                Content = new int[,]{}
            };
        }
    }
    
    public async Task<ResponseMessage<int[,]>> GetOpportunityMatrixById(long id)
    {
        try
        {
            var matrix = await repo.GetOpportunityCountsGrid(id);

            return new ResponseMessage<int[,]>
            {
                Code = 200,
                Content = matrix
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<int[,]>
            {
                Code = 500,
                Content = new int[,]{}
            };
        }
    }
    
    public async Task<ResponseMessage<Dictionary<int, int>>> GetRiskCountsByStatus(long id)
    {
        try
        {
            var matrix = await repo.GetRiskCountsByStatus(id);
            
            return new ResponseMessage<Dictionary<int, int>>
            {
                Code = 200,
                Content = matrix
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<Dictionary<int, int>>
            {
                Code = 500,
                Content = new Dictionary<int, int>()
            };
        }
    }
    
    public async Task<ResponseMessage<List<RiskAmountSummary>>> GetRiskAmountSummariesAsync(long id)
    {
        try
        {
            var list = await repo.GetRiskAmountSummariesAsync(id);
            
            return new ResponseMessage<List<RiskAmountSummary>>
            {
                Code = 200,
                Content = list
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<RiskAmountSummary>>
            {
                Code = 500,
                Content = []
            };
        }
    }
    
    public async Task<ResponseMessage<Dictionary<string, int>?>> GetRiskCountsByRupPhases(long id)
    {
        try
        {
            var matrix = await repo.GetRiskCountsByRupPhase(id);

            if (matrix == null)
            {
                return new ResponseMessage<Dictionary<string, int>?>
                {
                    Code = 404,
                    Content = new Dictionary<string, int>()
                };
            }
            return new ResponseMessage<Dictionary<string, int>?>
            {
                Code = 200,
                Content = matrix
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<Dictionary<string, int>?>
            {
                Code = 500,
                Content = new Dictionary<string, int>()
            };
        }
    }
}