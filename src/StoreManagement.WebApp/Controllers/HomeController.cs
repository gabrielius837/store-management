namespace StoreManagement.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _context;

    public HomeController(IHttpContextAccessor context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (_context.HttpContext is null)
            throw new ArgumentNullException(nameof(IHttpContextAccessor));
        var authenticated = _context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
        return authenticated ? RedirectToAction("Types", "Type") : View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
