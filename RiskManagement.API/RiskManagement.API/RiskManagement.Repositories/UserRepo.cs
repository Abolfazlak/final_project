using Microsoft.EntityFrameworkCore;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models.Users;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;

namespace RiskManagement.API.RiskManagement.Repositories;

public class UserRepo(RiskManagementDbContext context) : IUserRepo
{
    public async Task AddUserToDb(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
    
    public async Task<bool> CheckUserExists(string username)
    {
        return await context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
    }
    
    public async Task<User?> GetUserByUserName(string username)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower()
        && u.IsActive);
    }
    
    public async Task<User?> GetUserByّId(long? id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    }
    public async Task<Company?> GetCompanyByName(string companyName)
    {
        return await context.Companies.FirstOrDefaultAsync(u => u.CompanyName.ToLower() == companyName.ToLower());
    }
    
    public async Task<List<UserDto>> GetUsersByCompany(long companyId)
    {
        return await context.Users.Where(u => u.CompanyId == companyId && !u.IsAdmin)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            }).ToListAsync();
    }
    
    public async Task AddCompanyToDb(Company company)
    {
        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();
    }
}