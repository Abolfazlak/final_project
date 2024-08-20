using Carter;
using Microsoft.AspNetCore.Authorization;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;
using RiskManagement.API.RiskManagement.Models.Users;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Modules;

public class UserModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/registerAdmin", async (IUserService service, 
            ICommonHelper helper, AddAdminUserDto dto) =>
        {
            // var isMobileValid = helper.ValidateMobileNumber(dto.PhoneNumber);
            // if (!isMobileValid)
            // {
            //     return Results.BadRequest(new { Message = "ساختار شماره همراه اشتباه است!" });
            // }
            var isPasswordValid = helper.ValidatePassword(dto.Password);
            if (!isPasswordValid)
            {
                return Results.BadRequest(new { Message = "رمز عبور باید شامل حرف کوچک، بزرگ، عدد و علامت باشد!" });
            }
            var res = await service.RegisterAdmin(dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Message = res.Content }),
                400 => Results.BadRequest(new { Message = res.Content }),
                 _  => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/users/registerUser", [Authorize] async (IUserService service,
            HttpContext httpContext, AddUserDto dto) =>
        {
            var isAdmin = service.GetIsAdminFromHttpContext(httpContext);
            if (!isAdmin) return Results.Forbid();
            
            var companyId = service.GetCompanyIdFromHttpContext(httpContext);
            var res = await service.RegisterUser(dto, companyId);
            return res.Code switch
            {
                200 => Results.Ok(new { Message = res.Content }),
                400 => Results.BadRequest(new { Message = res.Content }),
                _  => Results.Problem(res.Content)
            };
        });
        
        app.MapPost("/users/login", async (IUserService service, LoginModel model) =>
        {
            var res = await service.Login(model);
            return res.Code switch
            {
                200 => Results.Ok(new {Data = res.Content}),
                401 => Results.Unauthorized(),
                _  => Results.Problem(res.Content?.Token)
            };
        });
        
        app.MapGet("/users/getAllUsers", async (IUserService service, HttpContext httpContext) =>
        {
            var res = await service.GetAllUsersService(httpContext);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapGet("/users/getUserById", async (IUserService service, long id) =>
        {
            var res = await service.GetUserByIdService(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapGet("/users/getUser", async (IUserService service, HttpContext httpContext) =>
        {
            var id = service.GetUserIdFromHttpContext(httpContext);
            var res = await service.GetUserByIdService(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapPost("/users/updateUser", async (IUserService service, UserDto dto) =>
        {
            var res = await service.UpdateUserService(dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapPost("/users/deactivateUser", async (IUserService service, long id) =>
        {
            var res = await service.UpdateUserService(id);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
        
        app.MapPost("/users/changePassword", async (IUserService service, PasswordDto dto) =>
        {
            var res = await service.ChangePassword(dto);
            return res.Code switch
            {
                200 => Results.Ok(new { Data = res.Content }),
                404 => Results.NotFound(),
                _  => Results.Problem("عملیات با خطا مواجه شد")
            };
        });
    }
}