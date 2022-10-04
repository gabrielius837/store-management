namespace StoreManagement.WebApp;

public interface IRoleUserResolver
{
    Task<RoleUser?> ResolveUser(string user);
}

public class RoleUserResolver : IRoleUserResolver
{
    private static readonly IDictionary<string, RoleUser> _cache = new Dictionary<string, RoleUser>()
    {
        { "administrator", new RoleUser("Manager", "administrator")},
        { "user", new RoleUser("Customer", "user")}
    };

    public Task<RoleUser?> ResolveUser(string user)
    {
        var exists = _cache.ContainsKey(user);
        var result = exists ? _cache[user] : null;
        return Task.FromResult(result);
    }
}

public record RoleUser(string Role, string User);