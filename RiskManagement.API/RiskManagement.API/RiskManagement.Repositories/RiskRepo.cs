using Microsoft.EntityFrameworkCore;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;

namespace RiskManagement.API.RiskManagement.Repositories;

public class RiskRepo(RiskManagementDbContext context) : IRiskRepo
{
    
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