namespace StoreManagement.WebApp;

public interface IProductCookieManager
{
    int[] ParseProductHistoryCookie(HttpContext context);
    int[] UpdateProductHistoryCookie(HttpContext context, int nextId);
}

public class ProductCookieManager : IProductCookieManager
{
    public const string PRODUCT_HISTORY = "ProductHistory";
    private const int MAX_ENTRIES = 3;

    public int[] ParseProductHistoryCookie(HttpContext context)
    {
        var cookie = context.Request.Cookies[ReadCookieName(context)];
        if (cookie is null)
            return Array.Empty<int>();
        
        var result = new List<int>();
        foreach(var str in cookie.Split('-'))
        {
            if (int.TryParse(str, out int id))
                result.Add(id);
        }

        return result.ToArray();
    }

    public int[] UpdateProductHistoryCookie(HttpContext context, int nextId)
    {
        var ids = ParseProductHistoryCookie(context);
        var queue = new Queue<int>(ids);
        queue.Enqueue(nextId);
        while (queue.Count > MAX_ENTRIES)
            queue.Dequeue();
        var cookieValue = string.Join('-', queue);
        var options = new CookieOptions()
        {
            HttpOnly = false,
            IsEssential = true
        };
        context.Response.Cookies.Append(ReadCookieName(context), cookieValue, options);
        return queue.ToArray();
    }

    private static string ReadCookieName(HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var result = string.Empty;
        if (claim is null)
            return PRODUCT_HISTORY;
        return $"{PRODUCT_HISTORY}-{claim.Value}";
    }
}