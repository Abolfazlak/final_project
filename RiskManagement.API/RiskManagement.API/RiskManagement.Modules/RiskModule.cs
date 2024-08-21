using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Modules;

public class RiskModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/risk/getMainRiskCategories", [Authorize] async (IRiskService service) =>
        {
            var res = await service.GetMainRiskCategories();
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapGet("/risk/getSecondaryRiskCategories", [Authorize] async (IRiskService service, int id) =>
        {
            var res = await service.GetSecondaryRiskCategories(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapGet("/risk/getRiskLists", [Authorize] async (IRiskService service, int id) =>
        {
            var res = await service.GetRiskList(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapPost("/risk/addRisk", [Authorize] async (IRiskService service, HttpContext httpContext, 
            RiskInputDto dto) =>
        {
            var res = await service.AddRiskService(httpContext, dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                400 => Results.BadRequest(),
                403 => Results.Forbid(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/risk/updateRisk", [Authorize] async (IRiskService service, HttpContext httpContext, 
            RiskUpdateDto dto) =>
        {
            var res = await service.UpdateRiskService(httpContext, dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/risk/deleteRisk", [Authorize] async (IRiskService service, HttpContext httpContext, 
            long id) =>
        {
            var res = await service.RemoveRiskService(httpContext, id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapGet("/risk/getAllRisksByProjectId", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            long id) =>
        {
            var res = await service.GetAllRiskByProjectId(httpContext, id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        
        app.MapPost("/risk/detail/addRiskDetail", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            RiskDetailsDto dto) =>
        {
            var res = await service.AddRiskDetailService(httpContext, dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/risk/detail/updateRiskDetail", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            [FromBody]RiskDetailUpdateDto dto) =>
        {
            var res = await service.UpdateRiskDetailService(httpContext, dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                _ => Results.Problem(res.Content)
            };
        });
        
        app.MapGet("risk/detail/getRiskDetail", [Authorize] async (IRiskService service, 
            HttpContext httpContext, 
            long id) =>
        {
            var res = await service.GetRiskDetailById(httpContext, id);
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