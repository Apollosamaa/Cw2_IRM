using Cw2.Helpers;
using Cw2.Models;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly AuthenticationHelper _authenticationHelper;

    public LoginController(AuthenticationHelper authenticationHelper)
    {
        _authenticationHelper = authenticationHelper;
    }

    public IActionResult Index()
    {
        var model = new LoginModel(); // Instantiate the model here
        return View("~/Views/Home/Login.cshtml", model); // Specify the path to the Login view
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model)
    {
        var isVerified = await _authenticationHelper.VerifyUserAsync(model.Email, model.Password);

        if (isVerified)
        {
            HttpContext.Session.SetString("email", model.Email); // Storing email in session for demonstration
            return RedirectToAction("Index", "Home"); // Redirect to Home/Index after successful login
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View("~/Views/Home/Login.cshtml", model); // Specify the path to the Login view
        }
    }
}
