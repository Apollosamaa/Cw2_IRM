using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace Cw2.Helpers
{
    public class AuthenticationHelper
    {
        private readonly HttpClient _httpClient;

        public AuthenticationHelper()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP2001/auth/api/");
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
                // Assuming the response structure is ["Verified", "True"]
                return result.Length == 2 && result[0] == "Verified" && result[1] == "True";
            }
            else
            {
                // Handle the failure scenario (e.g., log error, return false, etc.)
                return false;
            }
        }
    }
}
