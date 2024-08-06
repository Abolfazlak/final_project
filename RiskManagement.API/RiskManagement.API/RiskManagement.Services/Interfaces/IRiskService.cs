using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Risks;

namespace RiskManagement.API.RiskManagement.Services.Interfaces;

public interface IRiskService
{
    //RiskCategory
    public Task<ResponseMessage<List<MainRiskCategoryDto>>> GetMainRiskCategories();
    public Task<ResponseMessage<List<SecondaryRiskCategoryDto>>> GetSecondaryRiskCategories(int id);

    //Risks
    public Task<ResponseMessage<string>> AddRiskService(HttpContext httpContext, RiskInputDto risk);
    public Task<ResponseMessage<string>> UpdateRiskService(HttpContext httpContext, RiskUpdateDto risk);
    public Task<ResponseMessage<string>> RemoveRiskService(HttpContext httpContext, long id);
    public Task<ResponseMessage<List<RiskDto>>> GetAllRiskByProjectId(HttpContext httpContext, long id);
    
    //Risk Details
    public Task<ResponseMessage<string>> AddRiskDetailService(HttpContext httpContext, RiskDetailsDto riskDetails);
    public Task<ResponseMessage<string>> UpdateRiskDetailService(HttpContext httpContext, RiskDetailUpdateDto riskDetails);
    public Task<ResponseMessage<RiskDetails?>> GetRiskDetailById(HttpContext httpContext, long id);
}