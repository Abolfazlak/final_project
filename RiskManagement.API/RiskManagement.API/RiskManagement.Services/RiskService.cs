using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Helpers;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Services;

public class RiskService(IRiskRepo repo, IUserService userService) : IRiskService
{
    //Solution
    public async Task<ResponseMessage<string>> AddSolution(InputSolutionDto dto)
    {
        try
        {
            await repo.AddSolution(new Solution
            {
                Description = dto.Description,
                Amount = dto.Amount,
                RiskId = dto.RiskId,
            });
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد" + e.Message
            };
        }
    }
    public async Task<ResponseMessage<string>> UpdateSolution(UpdateSolutionDto dto)
    {
        try
        {
            var solution = await repo.GetSolutionsBySolutionId(dto.Id);
            if (solution == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "اطلاعات یافت نشد!"
                };
            }

            solution.Description = dto.Description;
            solution.Amount = dto.Amount;

            await repo.UpdateSolution(solution);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد" + e.Message
            };
        }
    }
    public async Task<ResponseMessage<string>> DeleteSolution(long id)
    {
        try
        {
            var solution = await repo.GetSolutionsBySolutionId(id);
            if (solution == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "اطلاعات یافت نشد!"
                };
            }
            await repo.RemoveSolution(solution);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد" + e.Message
            };
        }
    }
    public async Task<ResponseMessage<UpdateSolutionDto?>> GetSolutionById(long id)
    {
        try
        {
            var solution = await repo.GetSolutionsBySolutionId(id);
            if (solution == null)
            {
                return new ResponseMessage<UpdateSolutionDto?>
                {
                    Code = 404,
                    Content = null
                };
            }
            
            return new ResponseMessage<UpdateSolutionDto?>
            {
                Code = 200,
                Content = new UpdateSolutionDto
                {
                    Description = solution.Description,
                    Amount = solution.Amount,
                    RiskId = solution.RiskId,
                    Id = solution.Id
                }
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<UpdateSolutionDto?>
            {
                Code = 500,
                Content = null
            };
        }
    }
    public async Task<ResponseMessage<List<UpdateSolutionDto>>> GetAllSolutionsByRiskId(long id)
    {
        try
        {
            var solution = await repo.GetAllSolutions(id);
            if (solution.Count == 0)
            {
                return new ResponseMessage<List<UpdateSolutionDto>>
                {
                    Code = 404,
                    Content = []
                };
            }
            
            return new ResponseMessage<List<UpdateSolutionDto>>
            {
                Code = 200,
                Content = solution
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<UpdateSolutionDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }

    

    //Risk Status
    public async Task<ResponseMessage<List<RiskStatusDto>>> GetAllRiskStatusByProjectIdService(long id)
    {
        try
        {
            var res = await repo.GetAllRiskStatusByProjectId(id);
            if (res.Count == 0)
            {
                return new ResponseMessage<List<RiskStatusDto>>
                {
                    Code = 404,
                    Content = []
                };
            }

            return new ResponseMessage<List<RiskStatusDto>>
            {
                Code = 200,
                Content = res
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<RiskStatusDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }
    public async Task<ResponseMessage<List<RiskStatusDto>>> GetAllRiskStatusByProjectIdAnStatusService(long id, int status)
    {
        try
        {
            var res = await repo.GetAllRiskStatusByProjectIdAndStatus(id, status);
            if (res.Count == 0)
            {
                return new ResponseMessage<List<RiskStatusDto>>
                {
                    Code = 404,
                    Content = []
                };
            }

            return new ResponseMessage<List<RiskStatusDto>>
            {
                Code = 200,
                Content = res
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<RiskStatusDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }
    public async Task<ResponseMessage<string>> ChangeRiskStatus(InputRiskStatusDto dto)
    {
        try
        {
            var risk = await repo.GetRiskById(dto.Id);

            if (risk == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "ریسک کورد نظر یافت نشد!"
                };
            }
        
            if (dto.Status == 2)
            {
                if (!dto.IsNewSolution)
                {
                    risk.BestSolutionId = dto.SolutionId;
                }
                else
                {
                    await repo.AddSolution(new Solution
                    {
                        Description = dto.SolutionTitle!,
                        Amount = dto.FinalAmount,
                        RiskId = dto.Id,
                    });

                    var solutions = await repo.GetAllSolutions(dto.Id);
                    var sId = solutions.FirstOrDefault(s => s.Description == dto.SolutionTitle)!.Id;
                    risk.BestSolutionId = sId;
                }
            }

            risk.Status = dto.Status;
            risk.FinalAmount = dto.FinalAmount;
            risk.FinishedDate = dto.FinishedDate;

            await repo.UpdateRisk(risk);

            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد" + e.Message
            };
        }
        
    }

    //RiskCategory
    public async Task<ResponseMessage<List<MainRiskCategoryDto>>> GetMainRiskCategories()
    {
        try
        {
            var categories = await repo.GetMainRiskCategories();

            if (categories.Count == 0)
            {
                return new ResponseMessage<List<MainRiskCategoryDto>>
                {
                    Code = 404,
                    Content = []
                };
            }
            return new ResponseMessage<List<MainRiskCategoryDto>>
            {
                Code = 200,
                Content = categories
            };
            
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<MainRiskCategoryDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }

    public async Task<ResponseMessage<List<SecondaryRiskCategoryDto>>> GetSecondaryRiskCategories(int id)
    {
        try
        {
            var categories = await repo.GetSecondaryRiskCategories(id);

            if (categories.Count == 0)
            {
                return new ResponseMessage<List<SecondaryRiskCategoryDto>>
                {
                    Code = 404,
                    Content = []
                };
            }
            return new ResponseMessage<List<SecondaryRiskCategoryDto>>
            {
                Code = 200,
                Content = categories
            };
            
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<SecondaryRiskCategoryDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }

    //Risks
    public async Task<ResponseMessage<string>> AddRiskService(HttpContext httpContext, RiskInputDto risk)
    {
        try
        {
            if (CheckAccessToProject(httpContext, risk.ProjectId, out var responseMessage))
            {
                return responseMessage!;
            }

            var createRisk = CreateRisk(risk);

            await repo.AddRiskToDb(createRisk);

            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "ریسک با موفقیت اضافه شد."
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }
    }

    public async Task<ResponseMessage<string>> UpdateRiskService(HttpContext httpContext, RiskUpdateDto risk)
    {
        try
        {
            if (CheckAccessToProject(httpContext, risk.ProjectId, out var responseMessage))
            {
                return responseMessage!;
            }
            
            var getRisk = await repo.GetRiskById(risk.Id);
            if (getRisk == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "اطلاعات یافت نشد!"
                };
            }

            getRisk.Title = risk.Title;
            getRisk.SecondaryRiskCategoryId = risk.SecondaryRiskCategory.Id;

            await repo.UpdateRisk(getRisk);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "ریسک با موفقیت ویرایش شد."
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }
    }

    public async Task<ResponseMessage<string>> RemoveRiskService(HttpContext httpContext, long id)
    {
        try
        {
            var getRisk = await repo.GetRiskById(id);
            if (getRisk == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "اطلاعات یافت نشد!"
                };
            }
            
            if (CheckAccessToProject(httpContext, getRisk.ProjectId, out var responseMessage))
            {
                return responseMessage!;
            }

            var getRiskDetails = await repo.GetFullRiskDetailById(getRisk.Id);

            if (getRiskDetails != null)
            {
                await repo.RemoveRiskDetails(getRiskDetails);
            }
            
            await repo.RemoveRisk(getRisk);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "ریسک با موفقیت حذف شد."
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }

        
    }

    public async Task<ResponseMessage<List<RiskDto>>> GetAllRiskByProjectId(HttpContext httpContext, long id)
    {
        try
        {
            if (CheckAccessToProject(httpContext, id, out var responseMessage))
            {
                return new ResponseMessage<List<RiskDto>>
                {
                    Code = responseMessage!.Code,
                    Content = []
                };
            }
            
            var risks = await repo.GetAllRiskByProjectId(id);

            if (risks.Count == 0)
            {
                return new ResponseMessage<List<RiskDto>>
                {
                    Code = 404,
                    Content = []
                };
            }
            return new ResponseMessage<List<RiskDto>>
            {
                Code = 200,
                Content = risks
            };
            
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<RiskDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }
    
    
    
    //Risk Details
    public async Task<ResponseMessage<string>> AddRiskDetailService(HttpContext httpContext, 
        RiskDetailsDto riskDetails)
    {
        try
        {
            if (CheckAccessToProjectByRisk(httpContext, riskDetails.RiskId, out var responseMessage))
            {
                return responseMessage!;
            }
            
            const string documentUrl = "";
            var createRisk = CreateRiskDetails(riskDetails, documentUrl);

            await repo.AddRiskDetailToDb(createRisk);

            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "جزئیات ریسک با موفقیت اضافه شد."
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }
    }

    public async Task<ResponseMessage<string>> UpdateRiskDetailService(HttpContext httpContext, 
        RiskDetailUpdateDto riskDetails)
    {
        try
        {
            if (CheckAccessToProjectByRisk(httpContext, riskDetails.RiskId, out var responseMessage))
            {
                return responseMessage!;
            }
            
            const string documentUrl = "";
            var riskDetail = await repo.GetFullRiskDetailById(riskDetails.Id);

            if (riskDetail == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "اطلاعات یافت نشد!"
                };
            }

            riskDetail.Description = riskDetails.Description;
            riskDetail.RiskProbability = riskDetails.RiskProbability;
            riskDetail.RiskImpact = riskDetails.RiskImpact;
            riskDetail.RiskScore = !riskDetails.IsOpportunity ? riskDetails.RiskProbability * riskDetails.RiskImpact : 0;
            riskDetail.DocumentUrl = documentUrl;
            riskDetail.IsOpportunity = riskDetails.IsOpportunity;
            riskDetail.OpportunityImpact = riskDetails.OpportunityImpact;
            riskDetail.OpportunityProbability = riskDetails.OpportunityProbability;
            riskDetail.OpportunityScore = riskDetails.IsOpportunity ? 
                riskDetails.OpportunityImpact * riskDetails.OpportunityProbability
                : 0;
            riskDetail.RupPhase = riskDetails.RupPhase;
            riskDetail.EstimatedDateTime = riskDetails.EstimatedDateTime;
            riskDetail.EstimatedOpportunityAmount = riskDetails.EstimatedOpportunityAmount;
            riskDetails.EstimatedRiskAmount = riskDetails.EstimatedRiskAmount;

            await repo.UpdateRiskDetail(riskDetail);

            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "جزئیات ریسک با موفقیت ویرایش شد."
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }

    }

    public async Task<ResponseMessage<RiskDetailUpdateDto?>> GetRiskDetailById(HttpContext httpContext, long id)
    {
        try
        {
            var riskDetails = await repo.GetRiskDetailById(id);

            if (riskDetails != null &&
                CheckAccessToProjectByRisk(httpContext, riskDetails.RiskId, out var responseMessage))
            {
                return new ResponseMessage<RiskDetailUpdateDto?>
                {
                    Code = responseMessage!.Code,
                    Content = null
                };
            }
            
            if (riskDetails == null)
            {
                return new ResponseMessage<RiskDetailUpdateDto?>
                {
                    Code = 404,
                    Content = null
                };
            }
            return new ResponseMessage<RiskDetailUpdateDto?>
            {
                Code = 200,
                Content = riskDetails
            };
            
        }
        catch (Exception e)
        {
            return new ResponseMessage<RiskDetailUpdateDto?>
            {
                Code = 500,
                Content = null
            };
        }
    }
    
    
    
    
    /*
     *
     * This is private part :)
     * 
     */

    private bool CheckAccessToProject(HttpContext httpContext, long projectId, out ResponseMessage<string>? responseMessage)
    {
        var isAdmin = userService.GetIsAdminFromHttpContext(httpContext);
        if (isAdmin)
        {
            responseMessage = null;
            return false;
        }
        
        var userId = userService.GetUserIdFromHttpContext(httpContext);
        var permission = repo.CheckProjectAccess(userId, projectId).Result;

        if (!permission)
        {
            {
                responseMessage = new ResponseMessage<string>
                {
                    Code = 403,
                    Content = "شما به این پروژه دسترسی ندارید!"
                };
                return true;
            }
        }
        responseMessage = null;
        return false;
    }
    
    private bool CheckAccessToProjectByRisk(HttpContext httpContext, long riskId, 
        out ResponseMessage<string>? responseMessage)
    {
        var isAdmin = userService.GetIsAdminFromHttpContext(httpContext);
        if (isAdmin)
        {
            responseMessage = null;
            return false;
        }
        
        var userId = userService.GetUserIdFromHttpContext(httpContext);
        var permission = repo.CheckProjectAccessByRiskId(userId, riskId).Result;

        if (!permission)
        {
            {
                responseMessage = new ResponseMessage<string>
                {
                    Code = 403,
                    Content = "شما به این پروژه دسترسی ندارید!"
                };
                return true;
            }
        }
        responseMessage = null;
        return false;
    }
    
    private static Risk CreateRisk(RiskInputDto riskInputDto)
    {
        return new Risk
        {
            Title = riskInputDto.Title,
            SecondaryRiskCategoryId = riskInputDto.SecondaryRiskCategory.Id,
            ProjectId = riskInputDto.ProjectId,
            Status = 0,
        };
    }
    
    private static RiskDetails CreateRiskDetails(RiskDetailsDto riskInputDto, string documentUrl)
    {
        return new RiskDetails
        {
            Description = riskInputDto.Description,
            RupPhase = riskInputDto.RupPhase,
            IsOpportunity = riskInputDto.IsOpportunity,
            RiskProbability = riskInputDto.RiskProbability,
            RiskImpact = riskInputDto.RiskImpact,
            RiskScore = !riskInputDto.IsOpportunity ?  riskInputDto.RiskProbability * riskInputDto.RiskImpact : 0,
            OpportunityProbability = riskInputDto.OpportunityProbability,
            OpportunityImpact = riskInputDto.OpportunityImpact,
            OpportunityScore = riskInputDto.IsOpportunity ? 
                                riskInputDto.OpportunityProbability * riskInputDto.OpportunityImpact
                                : 0,
            EstimatedRiskAmount = riskInputDto.EstimatedRiskAmount,
            EstimatedOpportunityAmount = riskInputDto.EstimatedOpportunityAmount,
            DocumentUrl = documentUrl,
            EstimatedDateTime = riskInputDto.EstimatedDateTime,
            RiskId = riskInputDto.RiskId
        };
    }
}