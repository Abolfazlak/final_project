using Carter;
using Microsoft.AspNetCore.Authorization;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Modules;

public class RiskStatusModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/risk/status/getAllRiskStatusByProjectId", [Authorize] async (IRiskService service
        , long id) =>
        {
            var res = await service.GetAllRiskStatusByProjectIdService(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });

        
        app.MapGet("/risk/status/getAllRiskStatusByProjectIdAndStatus", [Authorize] async (IRiskService service
            , long id, int status) =>
        {
            var res = await service.GetAllRiskStatusByProjectIdAnStatusService(id, status);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapPost("/risk/status/changeRiskStatus", [Authorize] async (IRiskService service
            , InputRiskStatusDto dto) =>
        {
            var res = await service.ChangeRiskStatus(dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                403 => Results.Forbid(),
                404 => Results.NotFound(),
                _ => Results.Problem(res.Content)
            };
        });

    }
}