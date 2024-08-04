using Microsoft.EntityFrameworkCore;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;

namespace RiskManagement.API.RiskManagement.Repositories;

public class ProjectRepo(RiskManagementDbContext context) : IProjectRepo
{
    public async Task<List<Project>> GetAllProjectsForAdmin(long companyId)
    {
        return await context.Projects.Where(p => p.CompanyId == companyId).ToListAsync();
    }
    
    public async Task<List<Project>> GetAllProjectsByUserId(long userId)
    {
        return await context.Projects.Where(p => p.AssigneeUserId == userId).ToListAsync();
    }

    public async Task AddProjectToDb(Project project)
    {
        await context.Projects.AddAsync(project);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateProject(Project project)
    {
        context.Projects.Update(project);
        await context.SaveChangesAsync();
    }
    
    public async Task<Project?> GetProjectById(long projectId)
    {
        return await context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }
    
    public async Task DeleteProject(Project project)
    {
         context.Projects.Remove(project);
         await context.SaveChangesAsync();
    }
}