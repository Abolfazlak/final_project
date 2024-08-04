using RiskManagement.API.RiskManagement.Helpers;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;

namespace RiskManagement.API.RiskManagement.Middlewares;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class ExtractUsernameMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var userId = TokenHelper.GetUserIdFromToken(token);
            var isAdmin = TokenHelper.GetUserIsAdminFromToken(token);
            var companyId = TokenHelper.GetUCompanyIdFromToken(token);

            if (userId != null)
            {
                context.Items["userId"] = userId;
            }
            
            if (isAdmin != null)
            {
                context.Items["isAdmin"] = isAdmin;
            }
            
            if (companyId != null)
            {
                context.Items["companyId"] = companyId;
            }
        }

        await next(context);
    }
}
