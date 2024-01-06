using Cw2.Helpers;
using Cw2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                HttpContext.Session.SetInt32("profileId", userProfile.ProfileId);
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

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var profileId = HttpContext.Session.GetInt32("profileId");

        if (!profileId.HasValue)
        {
            return RedirectToAction("Login", "Home");
        }

        var userProfile = await _authenticationHelper.GetUserProfileAsync(profileId.Value);

        if (userProfile != null)
        {
            // Fetch the locations from your API
            var locations = await _authenticationHelper.GetLocationsAsync();

            // Create a SelectList with the locations and set the selected value
            ViewBag.Locations = new SelectList(locations, "LocationId", "LocationName", userProfile.ProfileLocation);

            return View("~/Views/Home/EditProfile.cshtml", userProfile);
        }

        return View("~/Views/Home/ProfileNotFound.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(Profile updatedProfile)
    {
        var profileId = HttpContext.Session.GetInt32("profileId");

        if (!profileId.HasValue)
        {
            return RedirectToAction("Login", "Home");
        }

        var userProfile = await _authenticationHelper.GetUserProfileAsync(profileId.Value);

        if (userProfile != null)
        {
            userProfile.ProfileFname = updatedProfile.ProfileFname;
            userProfile.ProfileLname = updatedProfile.ProfileLname;
            userProfile.ProfileWeight = updatedProfile.ProfileWeight;
            userProfile.ProfileHeight = updatedProfile.ProfileHeight;
            userProfile.ProfileDob = updatedProfile.ProfileDob;
            userProfile.ProfileLocation = updatedProfile.ProfileLocation;
            userProfile.ProfileBio = updatedProfile.ProfileBio; // Add this line to update the bio

            // The UserId, ProfileArchive, and ProfileCreatedAt fields should remain unchanged
            // They are already part of the userProfile object retrieved from the GetUserProfileAsync method

            // Send a PUT request to your API to update the profile
            var response = await _authenticationHelper.UpdateProfileInAPI(userProfile);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewProfile");
            }
            else
            {
                // Handle the error (e.g., display an error message to the user)
            }
        }

        return RedirectToAction("Index", "Home");
    }
}
