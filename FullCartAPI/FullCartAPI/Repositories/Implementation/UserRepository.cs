using Microsoft.EntityFrameworkCore;
using FullCartAPI.Data;
using FullCartAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FullCartAPI.Models;
using System.Text.RegularExpressions;
using System.Text;
using FullCartAPI.Helpers;

namespace FullCartAPI.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public UserRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUser()
        {
            try
            {
                return await _dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Log catchError = new Log
                {
                    Container = "UserRepository",
                    FunctionName = "AddUser",
                    ErrorMessage = ex.Message,
                    CreatedAt = DateTime.Now,
                };
                await _dbContext.AddAsync(catchError);
                await _dbContext.SaveChangesAsync();

                Console.WriteLine(catchError.ToString());
                return null;
            }

        }


        public async Task AddUser([FromBody] User userObj)
        {
            try
            {
                userObj.Password = PasswordHasher.HashPassword(userObj.Password);
                userObj.Role = "User";
                userObj.Token = "";
                userObj.CreatedAt=DateTime.Now;
                await _dbContext.AddAsync(userObj);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Log catchError=new Log
                {
                    Container = "UserRepository",
                    FunctionName = "AddUser",
                    ErrorMessage = ex.Message,
                    CreatedAt = DateTime.Now,
                };
                await _dbContext.AddAsync(catchError);
                await _dbContext.SaveChangesAsync();

                Console.WriteLine(catchError.ToString());
            }
         
        }

        private Task<bool> CheckEmailExistAsync(string? email)
            => _dbContext.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckUsernameExistAsync(string? username)
            => _dbContext.Users.AnyAsync(x => x.Email == username);

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }
    }
}
