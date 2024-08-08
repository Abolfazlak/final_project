using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models.Risks;

namespace RiskManagement.API.RiskManagement.Repositories.Interfaces;

public interface IRiskRepo
{
    //RiskSolution
    public Task<List<UpdateSolutionDto>> GetAllSolutions(long id);
    public Task<Solution?> GetSolutionsBySolutionId(long id);
    public Task UpdateSolution(Solution solution);
    public Task RemoveSolution(Solution solution);
    public Task AddSolution(Solution solution);


    //RiskCategory
    public Task<List<MainRiskCategoryDto>> GetMainRiskCategories();
    public Task<List<SecondaryRiskCategoryDto>> GetSecondaryRiskCategories(int id);

    //Risks
    public Task AddRiskToDb(Risk risk);
    public Task UpdateRisk(Risk risk);
    public Task RemoveRisk(Risk risk);
    public Task<Risk?> GetRiskById(long id);
    public Task<List<RiskDto>> GetAllRiskByProjectId(long id);
    
    //Risk Details
    public Task AddRiskDetailToDb(RiskDetails riskDetails);
    public Task UpdateRiskDetail(RiskDetails riskDetails);
    public Task RemoveRiskDetails(RiskDetails risk);
    public Task<RiskDetails?> GetRiskDetailById(long id);
    
    
    //Risk Status
    public Task<List<RiskStatusDto>> GetAllRiskStatusByProjectId(long id);
    public Task<List<RiskStatusDto>> GetAllRiskStatusByProjectIdAndStatus(long id, int status);
    
    
    //Access
    public Task<bool> CheckProjectAccess(long userId, long projectId);
    public Task<bool> CheckProjectAccessByRiskId(long userId, long riskId);


}