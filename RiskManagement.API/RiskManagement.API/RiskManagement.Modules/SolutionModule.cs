using Carter;
using Microsoft.AspNetCore.Authorization;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Modules;

public class SolutionModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapPost("/risk/solution/addSolution", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            InputSolutionDto dto) =>
        {
            var res = await service.AddSolution(dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/risk/solution/updateSolution", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            UpdateSolutionDto dto) =>
        {
            var res = await service.UpdateSolution(dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/risk/solution/deleteSolution", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            long id) =>
        {
            var res = await service.DeleteSolution(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapGet("/risk/solution/getSolutionById", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            long id) =>
        {
            var res = await service.GetSolutionById(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapGet("/risk/solution/getAllSolutionByRiskId", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            long id) =>
        {
            var res = await service.GetAllSolutionsByRiskId(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
    }

    
}