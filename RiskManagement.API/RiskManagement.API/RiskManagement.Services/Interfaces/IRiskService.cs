using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Risks;

namespace RiskManagement.API.RiskManagement.Services.Interfaces;

public interface IRiskService
{
    //Solution
    public Task<ResponseMessage<string>> AddSolution(InputSolutionDto dto);
    public Task<ResponseMessage<string>> UpdateSolution(UpdateSolutionDto dto);
    public Task<ResponseMessage<string>> DeleteSolution(long id);
    public Task<ResponseMessage<UpdateSolutionDto?>> GetSolutionById(long id);
    public Task<ResponseMessage<List<UpdateSolutionDto>>> GetAllSolutionsByRiskId(long id);

    
    //Risk Status
    public Task<ResponseMessage<List<RiskStatusDto>>> GetAllRiskStatusByProjectIdService(long id);
    public Task<ResponseMessage<List<RiskStatusDto>>> GetAllRiskStatusByProjectIdAnStatusService(long id, int status);
    public Task<ResponseMessage<string>> ChangeRiskStatus(InputRiskStatusDto dto);
    
    
    //RiskCategory
    public Task<ResponseMessage<List<MainRiskCategoryDto>>> GetMainRiskCategories();
    public Task<ResponseMessage<List<SecondaryRiskCategoryDto>>> GetSecondaryRiskCategories(int id);
    public Task<ResponseMessage<List<string>>> GetRiskList(int id);


    //Risks
    public Task<ResponseMessage<string>> AddRiskService(HttpContext httpContext, RiskInputDto risk);
    public Task<ResponseMessage<string>> UpdateRiskService(HttpContext httpContext, RiskUpdateDto risk);
    public Task<ResponseMessage<string>> RemoveRiskService(HttpContext httpContext, long id);
    public Task<ResponseMessage<List<RiskDto>>> GetAllRiskByProjectId(HttpContext httpContext, long id);
    
    //Risk Details
    public Task<ResponseMessage<string>> AddRiskDetailService(HttpContext httpContext, RiskDetailsDto riskDetails);
    public Task<ResponseMessage<string>> UpdateRiskDetailService(HttpContext httpContext, RiskDetailUpdateDto riskDetails);
    public Task<ResponseMessage<RiskDetailUpdateDto?>> GetRiskDetailById(HttpContext httpContext, long id);
}