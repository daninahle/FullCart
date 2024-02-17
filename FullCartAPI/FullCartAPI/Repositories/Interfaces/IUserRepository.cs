using FullCartAPI.Models;

namespace FullCartAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser();

    }
}
