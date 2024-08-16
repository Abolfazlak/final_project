using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models.Users;

namespace RiskManagement.API.RiskManagement.Repositories.Interfaces;

public interface IUserRepo
{
    public Task AddUserToDb(User? user);
    public Task AddCompanyToDb(Company company);
    public Task<bool> CheckUserExists(string username);
    public Task<User?> GetUserByUserName(string username);
    public Task<List<UserDto>> GetUsersByCompany(long companyId);
    public Task<Company?> GetCompanyByName(string companyName);
    public Task<User?> GetUserByّId(long? id);
    public Task UpdateUser(User user);

}