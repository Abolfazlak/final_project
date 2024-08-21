using System.IdentityModel.Tokens.Jwt;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Users;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Services;

public class UserService(ICommonHelper helper, IUserRepo userRepo) : IUserService
{
    public async Task<ResponseMessage<string>> RegisterAdmin(AddAdminUserDto dto)
    {
        try
        {
            var isUserExist = await userRepo.CheckUserExists(dto.UserName);
            if (isUserExist)
            {
                return new ResponseMessage<string>
                {
                    Code = 400,
                    Content = "نام کاربری تکراری است."
                };
            }

            var getCompany = await userRepo.GetCompanyByName(dto.CompanyName);
            if (getCompany == null)
            {
                var company = CreateCompany(dto.CompanyName);
                await userRepo.AddCompanyToDb(company);
                getCompany = await userRepo.GetCompanyByName(dto.CompanyName);
            }

            if (getCompany == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 500,
                    Content = "عملیات با خطا مواجه شد!"
                };
            }
            var user = CreateAdminUser(dto, getCompany);
            await userRepo.AddUserToDb(user);
        
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
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }
        
    }

    public async Task<ResponseMessage<string>> RegisterUser(AddUserDto dto, long companyId)
    {
        try
        {
            var isUserExist = await userRepo.CheckUserExists(dto.UserName);
            if (isUserExist)
            {
                return new ResponseMessage<string>
                {
                    Code = 400,
                    Content = "نام کاربری تکراری است."
                };
            }
            
            var user = CreateNormalUser(dto, companyId);
            await userRepo.AddUserToDb(user);
        
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
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }
        
    }

    public async Task<ResponseMessage<LoginOutputDto>> Login(LoginModel model)
    {
        try
        {
            var user = await userRepo.GetUserByUserName(model.Username);
            if (user == null)
            {
                return new ResponseMessage<LoginOutputDto>
                {
                    Code = 401,
                    Content = new LoginOutputDto
                    {
                        Token =  "نام کاربری یا گذرواژه اشتباه است.",
                        IsAdmin = false
                    }
                };
            }

            var checkPass = CheckPassword(model.Password, user.Password);

            if (!checkPass)
            {
                return new ResponseMessage<LoginOutputDto>
                {
                    Code = 401,
                    Content = new LoginOutputDto
                    {
                        Token =  "نام کاربری یا گذرواژه اشتباه است.",
                        IsAdmin = false
                    }
                };
            }

            var token = helper.GenerateAccessToken(user.Id, user.IsAdmin, user.CompanyId);
        
            return new ResponseMessage<LoginOutputDto>
            {
                Code = 200,
                Content = new LoginOutputDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    IsAdmin = user.IsAdmin
                }
                
            };

        }
        catch (Exception e)
        {
            return new ResponseMessage<LoginOutputDto>
            {
                Code = 500,
                Content = new LoginOutputDto
                {
                    Token =  "عملیات با خطا مواجه شد!" + e.Message,
                    IsAdmin = false
                }
            };
        }
        
    }

    public async Task<ResponseMessage<List<UserDto>>> GetAllUsersService(HttpContext httpContext)
    {
        try
        {
            var companyId = GetCompanyIdFromHttpContext(httpContext);
            var users = await userRepo.GetUsersByCompany(companyId);

            return new ResponseMessage<List<UserDto>>
            {
                Code = 200,
                Content = users
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<List<UserDto>>
            {
                Code = 500,
                Content = []
            };
        }
    }
    
    public async Task<ResponseMessage<UserDto>> GetUserByIdService(long? id)
    {
        try
        {
            var user = await userRepo.GetUserByّId(id);

            if (user == null)
            {
                return new ResponseMessage<UserDto>
                {
                    Code = 404,
                    Content = null
                };
            }

            return new ResponseMessage<UserDto>
            {
                Code = 200,
                Content = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                }
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<UserDto>
            {
                Code = 500,
                Content = null
            };
        }
    }
    
    public async Task<ResponseMessage<string>> UpdateUserService(UserDto dto)
    {
        try
        {
            var user = await userRepo.GetUserByّId(dto.Id);

            if (user == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = ""
                };
            }

            user.Email = dto.Email;
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            await userRepo.UpdateUser(user);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد!"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!"
            };
        }
    }
    
    public async Task<ResponseMessage<string>> UpdateUserService(long id)
    {
        try
        {
            var user = await userRepo.GetUserByّIdWithoutStatus(id);

            if (user == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = ""
                };
            }

            user.IsActive = !user.IsActive;

            await userRepo.UpdateUser(user);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد!"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!" + e.Message
            };
        }
    }


    public async Task<ResponseMessage<string>> ChangePassword(PasswordDto dto)
    {
        try
        {
            var user = await userRepo.GetUserByّId(dto.Id);

            if (user == null)
            {
                return new ResponseMessage<string>
                {
                    Code = 404,
                    Content = ""
                };
            }
            
            var checkPass = CheckPassword(dto.OldPassword, user.Password);

            if (!checkPass)
            {
                return new ResponseMessage<string>
                {
                    Code = 401,
                    Content = "گذرواژه را بدرستی وارد نمایید"
                };
            }
            var pass = helper.GetHash(dto.NewPassword);

            user.Password = pass;

            await userRepo.UpdateUser(user);
            
            return new ResponseMessage<string>
            {
                Code = 200,
                Content = "عملیات با موفقیت انجام شد!"
            };
        }
        catch (Exception e)
        {
            return new ResponseMessage<string>
            {
                Code = 500,
                Content = "عملیات با خطا مواجه شد!"
            };
        }
    }

    
    /*
     *
     * get data from jwt world :)
     *
     */
    
    public long GetCompanyIdFromHttpContext(HttpContext httpContext)
    {
        return httpContext.Items["companyId"] is string id ? long.Parse(id) : 0;
    }
    
    public long GetUserIdFromHttpContext(HttpContext httpContext)
    {
        return httpContext.Items["userId"] is string id ? long.Parse(id) : 0;
    }
    
    public bool GetIsAdminFromHttpContext(HttpContext httpContext)
    {
        var isAdmin = httpContext.Items["isAdmin"] as string;
        return isAdmin != null && isAdmin.Equals("true", StringComparison.CurrentCultureIgnoreCase);
    }
    
    
    /*
     *
     * this is private part of class :)
     * 
     */
    
    
    private bool CheckPassword(string inputPassword, string actualPassword)
    {
        var hashPass = helper.GetHash(inputPassword);
        return hashPass == actualPassword;
    }

    private static Company CreateCompany(string companyName)
    {
        return new Company
        {
            CompanyName = companyName
        };
    }
    private User CreateAdminUser(AddAdminUserDto dto, Company? company)
    {
        var pass = helper.GetHash(dto.Password);
        return new User
        {
            UserName = dto.UserName,
            Password = pass,
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = " ",
            IsAdmin = true,
            CompanyId = company!.Id,
            IsActive = true
        };
    }
    
    private User CreateNormalUser(AddUserDto dto, long companyId)
    {
        var pass = helper.GetHash(dto.Password);
        return new User
        {
            UserName = dto.UserName,
            Password = pass,
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = " ",
            IsAdmin = false,
            CompanyId = companyId,
            IsActive = true
        };
    }
}