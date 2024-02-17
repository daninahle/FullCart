using FullCartAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FullCartAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser();
        Task AddUser([FromBody] User userObj);
        Task<bool> CheckEmailExistAsync(string? email);
        Task<bool> CheckUsernameExistAsync(string? username);
        string CheckPasswordStrength(string pass);


    }
}
