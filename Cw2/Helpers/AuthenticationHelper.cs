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
        private readonly HttpClient _profileClient;
        private readonly ILogger<AuthenticationHelper> _logger;

        public AuthenticationHelper(ILogger<AuthenticationHelper> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP2001/auth/api/");
            _profileClient = new HttpClient();
            _profileClient.BaseAddress = new Uri("https://localhost:7088/api/");

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
            try
            {
                var profileResponse = await _profileClient.GetAsync($"Profiles/ByUserId/{userId}");

                if (profileResponse.IsSuccessStatusCode)
                {
                    var profileJsonString = await profileResponse.Content.ReadAsStringAsync();
                    var profile = JsonSerializer.Deserialize<Profile>(profileJsonString);

                    if (profile != null && profile.ProfileLocation.HasValue)
                    {
                        var locationResponse = await _profileClient.GetAsync($"Locations/{profile.ProfileLocation}");

                        if (locationResponse.IsSuccessStatusCode)
                        {
                            var locationJsonString = await locationResponse.Content.ReadAsStringAsync();
                            var location = JsonSerializer.Deserialize<Location>(locationJsonString);

                            if (location != null)
                            {
                                profile.ProfileLocationNavigation = location;
                                _logger.LogInformation("User profile found successfully with location details.");
                            }
                            else
                            {
                                _logger.LogError($"Location not found for location ID: {profile.ProfileLocation}");
                            }
                        }
                        else
                        {
                            _logger.LogError($"Error fetching location. StatusCode: {locationResponse.StatusCode}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Profile not found for user ID: {userId} or location ID is missing.");
                    }

                    return profile; // Return the profile, regardless of location details
                }
                else
                {
                    _logger.LogError($"Error fetching user profile. StatusCode: {profileResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while fetching user profile: {ex.Message}");
            }

            return null;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            var response = await _profileClient.GetAsync("locations");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var locations = JsonSerializer.Deserialize<List<Location>>(jsonString);
                return locations;
            }

            return null;
        }

        public async Task<HttpResponseMessage> UpdateProfileInAPI(Profile updatedProfile)
        {
            var jsonString = JsonSerializer.Serialize(updatedProfile);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _profileClient.PutAsync($"profiles/{updatedProfile.ProfileId}", httpContent);

            return response;
        }

    }
}
