using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Users;

namespace RiskManagement.API.RiskManagement.Services.Interfaces;

public interface IUserService
{
    public Task<ResponseMessage<string>> RegisterAdmin(AddAdminUserDto dto);
    public Task<ResponseMessage<string>> RegisterUser(AddUserDto dto, long companyId);
    public Task<ResponseMessage<LoginOutputDto>> Login(LoginModel model);
    public Task<ResponseMessage<List<UserDto>>> GetAllUsersService(HttpContext httpContext);
    public Task<ResponseMessage<UserDto>> GetUserByIdService(long? id);
    public Task<ResponseMessage<string>> UpdateUserService(UserDto dto);
    public Task<ResponseMessage<string>> ChangePassword(PasswordDto dto);

    
    public long GetCompanyIdFromHttpContext(HttpContext httpContext);
    public long GetUserIdFromHttpContext(HttpContext httpContext);

    public bool GetIsAdminFromHttpContext(HttpContext httpContext);

}