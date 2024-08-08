using Carter;
using Microsoft.AspNetCore.Authorization;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;
using RiskManagement.API.RiskManagement.Models.Projects;
using RiskManagement.API.RiskManagement.Models.Users;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Modules;

public class ProjectModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("/project/getAllProjects", [Authorize] async (IProjectService service,
            HttpContext httpContext) =>
        {
            var res = await service.GetAllProjectsService(httpContext);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapPost("/project/createProject", [Authorize] async (IUserService userService, 
            IProjectService service, HttpContext httpContext, CreateProjectDto dto) =>
        {
            var isAdmin = userService.GetIsAdminFromHttpContext(httpContext);
            if (!isAdmin) return Results.Forbid();
            
            var res = await service.CreateProjectService(dto, httpContext);
            return res.Code switch
            {
                200 => Results.Ok(new { Message = res.Content }),
                _  => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/project/updateProject", [Authorize] async (IUserService userService, 
            IProjectService service, HttpContext httpContext, UpdateProjectDto dto) =>
        {
            var isAdmin = userService.GetIsAdminFromHttpContext(httpContext);
            if (!isAdmin) return Results.Forbid();
            
            var res = await service.UpdateProjectService(dto, httpContext);
            return res.Code switch
            {
                200 => Results.Ok(new { Message = res.Content }),
                _  => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/project/deleteProject", [Authorize] async (IUserService userService, 
            IProjectService service, HttpContext httpContext, long id) =>
        {
            var isAdmin = userService.GetIsAdminFromHttpContext(httpContext);
            if (!isAdmin) return Results.Forbid();
            
            var res = await service.DeleteProjectService(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Message = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem(res.Content)
            };
        });
        
        app.MapGet("/project/getProjectById", [Authorize] async (IProjectService service, long id) =>
        {
            var res = await service.GetProjectByIdService(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
    }
}