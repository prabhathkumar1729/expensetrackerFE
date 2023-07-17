using BlazorWalletTrackerFE.Shared.Models;
namespace BlazorWalletTrackerFE.Shared.Services
{
    public interface IUserService
    {
        Task<User> GetUser();
    }
}
