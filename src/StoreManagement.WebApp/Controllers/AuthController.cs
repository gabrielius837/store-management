

namespace StoreManagement.WebApp.Controllers;

public class AuthController : Controller
{
    private readonly IRoleUserResolver _resolver;

    public AuthController(IRoleUserResolver resolver)
    {
        _resolver = resolver;
    }

    [HttpPost]
    public async Task<IActionResult> Login(string user)
    {
        var roleUser = await _resolver.ResolveUser(user);

        if (roleUser is null)
            return RedirectToAction("/Error");

        Claim[] claims =
        {
            new Claim(ClaimTypes.Role, roleUser.Role),
            new Claim(ClaimTypes.Name, roleUser.User)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "login");
        var principal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToAction("Types", "Type");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
