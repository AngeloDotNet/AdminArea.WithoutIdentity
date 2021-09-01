using System.Threading.Tasks;
using AspNetCore_SimpleLogin.Models.ViewModels;

namespace AspNetCore_SimpleLogin.Models.Services.Application
{
    public interface IUserService
    {
        Task<bool> IsLoginCorrectAsync(string username, string password);
        Task<UserDetailViewModel> GetUserAsync(string username);
        string PasswordSalt(string password);
    }
}