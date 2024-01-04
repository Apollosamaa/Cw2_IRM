using Cw2.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
namespace Cw2.Helpers
{
    public class AuthenticationHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationHelper> _logger;

        public AuthenticationHelper(ILogger<AuthenticationHelper> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP2001/auth/api/");
            _logger = logger;
        }

        public async Task<bool> VerifyUserAsync(string email, string password)
        {
            var content = new StringContent(
                $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}",
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("users", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<string[]>();
                return result.Length == 2 && result[0] == "Verified" && result[1] == "True";
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> VerifyUserAgainstDatabaseAsync(string email, string password)
        {
            var response = await _httpClient.GetAsync("https://localhost:7088/api/users"); // Update URL when publishing
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<User>>(jsonString);
                var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
                return user != null;
            }
            else
            {
                return false;
            }
        }

        public async Task<int?> GetUserIdAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7088/api/Users/ByEmail?email={email}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<User>(jsonString);

                    if (user != null)
                    {
                        return user.UserId;
                    }
                    else
                    {
                        _logger.LogError($"User not found for {email}");
                    }
                }
                else
                {
                    _logger.LogError($"Error fetching user ID for {email}. StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while fetching user ID for {email}: {ex.Message}");
            }
            return null;
        }

        public async Task<Profile> GetUserProfileAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/profiles?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var profiles = JsonSerializer.Deserialize<List<Profile>>(jsonString);
                return profiles.FirstOrDefault(p => p.UserId == userId);
            }
            return null;
        }
    }
}
