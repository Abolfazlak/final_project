using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Projects;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Services;

public class ProjectService(IProjectRepo projectRepo, IUserService userService) : IProjectService
{
    public async Task<ResponseMessage<List<ProjectDto>>> GetAllProjectsService(HttpContext httpContext)
    {
        try
        {
            List<Project> res;
            var isAdmin = userService.GetIsAdminFromHttpContext(httpContext);
            if (isAdmin)
            {
                var companyId = userService.GetCompanyIdFromHttpContext(httpContext);
                res = await projectRepo.GetAllProjectsForAdmin(companyId);

                if (res.Count == 0)
                {
                    return new ResponseMessage<List<ProjectDto>>
                    {
                        Code = 404,
                        Content = []
                    };
                }

                return new ResponseMessage<List<ProjectDto>>
                {
                    Code = 200,
                    Content =
                        res.Select(p => new ProjectDto
                        {
                            Id = p.Id,
                            ProjectName = p.ProjectName,
                            Methodology = p.Methodology,
                            Description = p.Description
                        }).ToList()
                };
            }

            var userId = userService.GetUserIdFromHttpContext(httpContext);
            res = await projectRepo.GetAllProjectsByUserId(userId);

            if (res.Count == 0)
            {
                return new ResponseMessage<List<ProjectDto>>
                {
                    Code = 404,
                    Content = []
                };
            }

            return new ResponseMessage<List<ProjectDto>>
            {
                Code = 200,
                Content =
                    res.Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        ProjectName = p.ProjectName,
                        Methodology = p.Methodology,
                        Description = p.Description
                    }).ToList()
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<ProjectDto>>
            {
                Code = 500,
                Content = []
            };
        }

    }

    public async Task<ResponseMessage<string>> CreateProjectService(CreateProjectDto dto, HttpContext httpContext)
    {
        try
        {
            var companyId = userService.GetCompanyIdFromHttpContext(httpContext);
            var company = CreateProject(dto, companyId);

            await projectRepo.AddProjectToDb(company);

            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد."
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد." + e.Message
            };
        }
        
        
    }

    public async Task<ResponseMessage<ProjectWithAssigneeDto>> GetProjectByIdService(long id)
    {
        var project = await projectRepo.GetProjectById(id);
        if (project == null)
        {
            return new ResponseMessage<ProjectWithAssigneeDto>
            {
                Code = 404,
                Content = null
            };
        }
        if (project.AssigneeUserId == null)
                return new ResponseMessage<ProjectWithAssigneeDto>
                {
                    Code = 200,
                    Content = new ProjectWithAssigneeDto
                    {
                        Id = project.Id,
                        ProjectName = project.ProjectName,
                        Methodology = project.Methodology,
                        Description = project.Description,
                        UserDto = null
                    }
                };
            
        var userRes = await userService.GetUserByIdService(project.AssigneeUserId);
        var user = userRes.Content;

        return new ResponseMessage<ProjectWithAssigneeDto>
        {
            Code = 200,
            Content = new ProjectWithAssigneeDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Methodology = project.Methodology,
                Description = project.Description,
                UserDto = user
            }
        };

    }

    public async Task<ResponseMessage<string>> DeleteProjectService(long id)
    {
        try
        {
            var project = await projectRepo.GetProjectById(id);

            if (project == null)
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = "پروژه مورد نظر یافت نشد."
                };
            await projectRepo.DeleteProject(project);
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
                Content = "عملیات با خطا انجام شد" + e.Message
            };
        }
    }
    
    
    public async Task<ResponseMessage<string>> UpdateProjectService(UpdateProjectDto dto, HttpContext httpContext)
    {
        try
        {
            var companyId = userService.GetCompanyIdFromHttpContext(httpContext);
            var project = CreateProjectById(dto, companyId);
                
            await projectRepo.UpdateProject(project);
            
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
                Content = "عملیات با خطا انجام شد" + e.Message
            };
        }
    }
    
    
    /*
     *
     * this is private part of class :)
     *
     */
    
    private static Project CreateProject(CreateProjectDto dto, long companyId)
    {
        return new Project
        {
            ProjectName = dto.ProjectName,
            Methodology = dto.Methodology,
            Description = dto.Description,
            CompanyId = companyId,
            AssigneeUserId = dto.AssigneeUserId,
        };
    }
    
    private static Project CreateProjectById(UpdateProjectDto dto, long companyId)
    {
        return new Project
        {
            Id = dto.Id,
            ProjectName = dto.ProjectName,
            Methodology = dto.Methodology,
            Description = dto.Description,
            CompanyId = companyId,
            AssigneeUserId = dto.AssigneeUserId,
        };
    }
}

