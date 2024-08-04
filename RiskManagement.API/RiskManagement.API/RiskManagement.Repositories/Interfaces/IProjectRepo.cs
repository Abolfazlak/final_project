
using RiskManagement.API.RiskManagement.DataProvide;

namespace RiskManagement.API.RiskManagement.Repositories.Interfaces;

public interface IProjectRepo
{
    public Task<List<Project>> GetAllProjectsForAdmin(long companyId);
    public Task<List<Project>> GetAllProjectsByUserId(long userId);
    public Task AddProjectToDb(Project project);
    public Task UpdateProject(Project project);
    public Task<Project?> GetProjectById(long projectId);
    public Task DeleteProject(Project project);


}