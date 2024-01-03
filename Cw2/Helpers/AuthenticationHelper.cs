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
        private readonly ILogger<AuthenticationHelper> _logger; // Inject ILogger

        public AuthenticationHelper(ILogger<AuthenticationHelper> logger) // Inject ILogger in the constructor
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP2001/auth/api/");
            _logger = logger; // Assign injected ILogger
        }

        public async Task<bool> VerifyUserAsync(string email, string password)
        {
            _logger.LogInformation("Verifying user via API...");
            var content = new StringContent(
                $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}",
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("users", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<string[]>();
                _logger.LogInformation($"API Response: {string.Join(", ", result)}");
                return result.Length == 2 && result[0] == "Verified" && result[1] == "True";
            }
            else
            {
                // Handle the failure scenario (e.g., log error, return false, etc.)
                return false;
            }
        }

        public async Task<bool> VerifyUserAgainstDatabaseAsync(string email, string password)
        {
            _logger.LogInformation("Verifying user against database..."); // Log information
            var response = await _httpClient.GetAsync("https://localhost:7088/api/users"); // Update URL when publishing
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<User>>(jsonString);
                _logger.LogInformation($"Number of Users: {users.Count}");
                _logger.LogInformation($"All Users: {JsonSerializer.Serialize(users)}");
                // Check if the credentials match any user in the retrieved data
                var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
                _logger.LogInformation($"User Object: {JsonSerializer.Serialize(user)}");
                return user != null;
            }
            else
            {
                // Handle API errors or non-JSON responses
                // For example, you might return false or throw an exception
                return false;
            }
        }
    }
}
