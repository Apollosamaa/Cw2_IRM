using Cw2.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class ProfileController : Controller
{
    private readonly AuthenticationHelper _authenticationHelper;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(AuthenticationHelper authenticationHelper, ILogger<ProfileController> logger)
    {
        _authenticationHelper = authenticationHelper;
        _logger = logger;
    }

    public async Task<IActionResult> ViewProfile()
    {
        var userId = HttpContext.Session.GetInt32("userId");
        _logger.LogInformation($"Retrieving profile for UserID: {userId}");

        if (userId.HasValue)
        {
            var userProfile = await _authenticationHelper.GetUserProfileAsync(userId.Value);

            if (userProfile != null)
            {
                _logger.LogInformation($"Profile retrieved for UserID: {userId}. Profile data: {JsonSerializer.Serialize(userProfile)}");
                return View("~/Views/Home/Profile.cshtml", userProfile);
            }
            else
            {
                _logger.LogError("Profile not found for user ID: {userId}");
                return View("ProfileNotFound");
            }
        }
        else
        {
            _logger.LogError("User ID not found in session.");
            return RedirectToAction("Index", "Home");
        }
    }
}
