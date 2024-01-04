using Cw2.Helpers;
using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    private readonly AuthenticationHelper _authenticationHelper;

    public ProfileController(AuthenticationHelper authenticationHelper)
    {
        _authenticationHelper = authenticationHelper;
    }

    public async Task<IActionResult> ViewProfile()
    {
        var userId = HttpContext.Session.GetInt32("userId");
        if (userId.HasValue)
        {
            var userProfile = await _authenticationHelper.GetUserProfileAsync(userId.Value);
            return View(userProfile);
        }
        return RedirectToAction("Index", "Home");
    }

    // Other profile-related actions...
}
