namespace StoreManagement.WebApp;

public interface IProductSubtypeRepository
{
    Task<ProductSubtype[]> GetChildren(int id);
    Task<ProductSubtype?> GetSubtype(int id);
    Task<ProductSubtype[]> GetSubtypes();
}

public class ProductSubtypeRepository : IProductSubtypeRepository
{
    private readonly List<ProductSubtype> _cache;

    public ProductSubtypeRepository(List<ProductSubtype> cache)
    {
        _cache = cache;
    }

    public Task<ProductSubtype[]> GetChildren(int id)
    {
        var result = _cache.Where(x => x.TypeId == id).ToArray();
        return Task.FromResult(result);
    }

    public Task<ProductSubtype?> GetSubtype(int id)
    {
        var result = _cache.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(result);
    }

    public Task<ProductSubtype[]> GetSubtypes()
    {
        var result = _cache.ToArray();
        return Task.FromResult(result);
    }
}