namespace StoreManagement.WebApp;

public interface IProductTypeRepository
{
    Task<ProductType[]> GetAll();
}

public class ProductTypeRepository : IProductTypeRepository
{
    private readonly List<ProductType> _cache;

    public ProductTypeRepository(List<ProductType> cache)
    {
        _cache = cache;
    }

    public Task<ProductType[]> GetAll()
    {
        return Task.FromResult(_cache.ToArray());
    }
}