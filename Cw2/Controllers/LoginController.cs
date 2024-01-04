using Cw2.Helpers;
using Cw2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

public class LoginController : Controller
{
    private readonly AuthenticationHelper _authenticationHelper;
    private readonly ILogger<LoginController> _logger;

    public LoginController(AuthenticationHelper authenticationHelper, ILogger<LoginController> logger)
    {
        _authenticationHelper = authenticationHelper;
        _logger = logger; // Assign the injected ILogger
    }

    public IActionResult Index()
    {
        var model = new LoginModel(); // Instantiate the model here
        return View("~/Views/Home/Login.cshtml", model); // Specify the path to the Login view
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model)
    {
        try
        {
            var isVerifiedApi = await _authenticationHelper.VerifyUserAsync(model.Email, model.Password);
            var isVerifiedDb = await _authenticationHelper.VerifyUserAgainstDatabaseAsync(model.Email, model.Password);

            if (isVerifiedApi && isVerifiedDb)
            {
                var userId = await _authenticationHelper.GetUserIdAsync(model.Email);
                if (userId.HasValue)
                {
                    HttpContext.Session.SetString("email", model.Email);
                    HttpContext.Session.SetInt32("userId", userId.Value);
                    _logger.LogInformation($"Login successful for {model.Email}. Redirecting...");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogError($"Error: User ID not found for {model.Email}");
                }
            }
            else
            {
                _logger.LogError($"Error: Verification failed for {model.Email}");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View("~/Views/Home/Login.cshtml", model);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in LoginController: {ex.Message}");
            throw; // Or handle the exception as per your application's requirements
        }
    }
}
