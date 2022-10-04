namespace StoreManagement.WebApp.Controllers;

[Authorize]
public abstract class AuthorizedController : Controller
{
    protected AuthorizedController() {}

    protected virtual bool IsManager => HttpContext.User.HasClaim(claim => claim.Type == ClaimTypes.Role && claim.Value == "Manager");
}