using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Projects;

namespace RiskManagement.API.RiskManagement.Services.Interfaces;

public interface IProjectService
{
    public Task<ResponseMessage<List<ProjectDto>>> GetAllProjectsService(HttpContext httpContext);
    public Task<ResponseMessage<string>> CreateProjectService(CreateProjectDto dto, HttpContext httpContext);
    public Task<ResponseMessage<ProjectWithAssigneeDto>> GetProjectByIdService(long id);
    public Task<ResponseMessage<string>> UpdateProjectService(CreateProjectDto dto, HttpContext httpContext);
    public Task<ResponseMessage<string>> DeleteProjectService(long id);


}