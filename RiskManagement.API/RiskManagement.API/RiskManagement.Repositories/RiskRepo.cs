using Microsoft.EntityFrameworkCore;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;

namespace RiskManagement.API.RiskManagement.Repositories;

public class RiskRepo(RiskManagementDbContext context) : IRiskRepo
{
    
    /*
     *
     * This is Risk Status Part
     *
     */
    
    public async Task<List<RiskStatusDto>> GetAllRiskStatusByProjectId(long id)
    {
        var res = await context.Risks
            .Include(r => r.SecondaryRiskCategory)
            .Include(r => r.SecondaryRiskCategory.MainRiskCategory)
            .Include(r => r.RiskDetails)
            .Where(r => r.ProjectId == id)
            .Select(r => new RiskStatusDto
            {
                Id = r.Id,
                Title = r.Title,
                MainRiskCategory = new MainRiskCategoryDto
                {
                    Id = r.SecondaryRiskCategory.MainRiskCategory.Id,
                    Title = r.SecondaryRiskCategory.MainRiskCategory.Title
                },
                SecondaryRiskCategory = new SecondaryRiskCategoryDto
                {
                    Id = r.SecondaryRiskCategoryId,
                    Title = r.SecondaryRiskCategory.Title
                },
                Status = r.Status,
                EstimatedFinishedDate = r.RiskDetails.Count != 0 ? r.RiskDetails.First().EstimatedDateTime : default,
                FinishedDate = r.FinishedDate,
                
                EstimatedAmount =  r.RiskDetails.Count != 0 ? 
                    r.RiskDetails.First().IsOpportunity ? r.RiskDetails.First().EstimatedOpportunityAmount
                        : r.RiskDetails.First().EstimatedRiskAmount : 0,
                
                FinalAmount = r.FinalAmount,
                
                BestSolution = r.Solutions.Where(s => s.Id == r.BestSolutionId).Select(s => new UpdateSolutionDto
                {
                    Description = s.Description,
                    Amount = s.Amount,
                    RiskId = s.RiskId,
                    Id = s.Id
                }).FirstOrDefault()



            }).ToListAsync();
        
        return res;
    }

    public async Task<List<RiskStatusDto>> GetAllRiskStatusByProjectIdAndStatus(long id, int status)
    {
        var risks = await GetAllRiskStatusByProjectId(id);
        return risks.Where(r => r.Status == status).ToList();
    }

    
    /*
     *
     * This is Risk Category's Part
     *
     */
    public async Task<List<MainRiskCategoryDto>> GetMainRiskCategories()
    {
        return await context.MainRiskCategories.Select(mrc => new MainRiskCategoryDto
        {
            Id = mrc.Id,
            Title = mrc.Title
        }).ToListAsync();
    }
    
    public async Task<List<SecondaryRiskCategoryDto>> GetSecondaryRiskCategories(int id)
    {
        return await context.SecondaryRiskCategories
            .Where(src => src.MainRiskCategoryId == id)
            .Select(src => new SecondaryRiskCategoryDto
            {
                Id = src.Id,
                Title = src.Title
            }).ToListAsync();
    }
    
    /*
     *
     * This is Risk's Part
     * 
     */
    
    public async Task AddRiskToDb(Risk risk)
    {
        await context.Risks.AddAsync(risk);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRisk(Risk risk)
    {
        context.Risks.Update(risk);
        await context.SaveChangesAsync();
    }
    
    public async Task RemoveRisk(Risk risk)
    {
        context.Risks.Remove(risk);
        await context.SaveChangesAsync();
    }

    public async Task<Risk?> GetRiskById(long id)
    {
        return await context.Risks.FirstOrDefaultAsync(r => r.Id == id);
    }
    
    public async Task<List<RiskDto>> GetAllRiskByProjectId(long id)
    {
        var res = await context.Risks
            .Include(r => r.SecondaryRiskCategory)
            .Include(r => r.SecondaryRiskCategory.MainRiskCategory)
            .Where(r => r.ProjectId == id)
            .Select(r => new RiskDto
            {
                Id = r.Id,
                Title = r.Title,
                MainRiskCategory = new MainRiskCategoryDto
                {
                    Id = r.SecondaryRiskCategory.MainRiskCategory.Id,
                    Title = r.SecondaryRiskCategory.MainRiskCategory.Title
                },
                SecondaryRiskCategory = new SecondaryRiskCategoryDto
                {
                    Id = r.SecondaryRiskCategoryId,
                    Title = r.SecondaryRiskCategory.Title
                }
            }).ToListAsync();
        
        return res;
    }
    
    
    /*
     *
     * This is Risk Details' Part
     *
     */
    
    public async Task AddRiskDetailToDb(RiskDetails riskDetails)
    {
        await context.RiskDetails.AddAsync(riskDetails);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateRiskDetail(RiskDetails riskDetails)
    {
        context.RiskDetails.Update(riskDetails);
        await context.SaveChangesAsync();
    }
    
    public async Task RemoveRiskDetails(RiskDetails risk)
    {
        context.RiskDetails.Remove(risk);
        await context.SaveChangesAsync();
    }
    
    public async Task<RiskDetails?> GetRiskDetailById(long id)
    {
        return await context.RiskDetails.FirstOrDefaultAsync(rd => rd.RiskId == id);
    }
    
    /*
     *
     * This is Risk's Solution Part
     *
     */

    public async Task<List<UpdateSolutionDto>> GetAllSolutions(long id)
    {
        return await context.Solutions.Where(s => s.RiskId == id).Select(s => new UpdateSolutionDto
        {
            Description = s.Description,
            Amount = s.Amount,
            RiskId = s.RiskId,
            Id = s.Id
        }).ToListAsync();
    }
    
    public async Task<Solution?> GetSolutionsBySolutionId(long id)
    {
        return await context.Solutions.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateSolution(Solution solution)
    {
        context.Solutions.Update(solution);
        await context.SaveChangesAsync();
    }
    
    public async Task RemoveSolution(Solution solution)
    {
        context.Solutions.Remove(solution);
        await context.SaveChangesAsync();
    }
    
    public async Task AddSolution(Solution solution)
    {
        await context.Solutions.AddAsync(solution);
        await context.SaveChangesAsync();
    }
    
    /*
     *
     * Permission Check
     *
     */

    public async Task<bool> CheckProjectAccess(long userId, long projectId)
    {
        return await context.Projects.AnyAsync(p => p.Id == projectId && p.AssigneeUserId == userId);
    }
    
    public async Task<bool> CheckProjectAccessByRiskId(long userId, long riskId)
    {
        var projectId = context.Risks.FirstOrDefault(a => a.Id == riskId)?.ProjectId;
        return await context.Projects.AnyAsync(p => p.Id == projectId && p.AssigneeUserId == userId);
    }
}