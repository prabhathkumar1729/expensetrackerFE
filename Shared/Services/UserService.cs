using System.Net.Http.Json;
using BlazorWalletTrackerFE.Shared.Models;
namespace BlazorWalletTrackerFE.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<User> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<User>("api/User/getUserDetails/101");
        }
    }
}
