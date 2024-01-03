using Cw2.Helpers;
using Cw2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        var isVerifiedApi = await _authenticationHelper.VerifyUserAsync(model.Email, model.Password);
        var isVerifiedDb = await _authenticationHelper.VerifyUserAgainstDatabaseAsync(model.Email, model.Password);
        _logger.LogInformation($"isVerifiedApi: {isVerifiedApi}");
        _logger.LogInformation($"isVerifiedDb: {isVerifiedDb}");

        if (isVerifiedApi && isVerifiedDb)
        {
            HttpContext.Session.SetString("email", model.Email);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View("~/Views/Home/Login.cshtml", model);
        }
    }

}
