namespace StoreManagement.WebApp;

public interface IProductRepository
{
    Task<Product[]> GetProducts(int subtypeId);
    Task<Product[]> GetProducts(int[] ids);
    Task<Product?> GetProduct(int id);
    Task<bool> CreateProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(int id);
}

public class ProductRepository : IProductRepository
{
    static readonly object _object = new object();
    private readonly List<Product> _cache;
    private readonly IProductSubtypeRepository _subtypeRepository;

    public ProductRepository(List<Product> cache, IProductSubtypeRepository subtypeRepository)
    {
        _cache = cache;
        _subtypeRepository = subtypeRepository;
    }

    public async Task<bool> CreateProduct(Product product)
    {
        var subtype = await _subtypeRepository.GetSubtype(product.SubtypeId);
        if (subtype is null)
            return false;
        lock (_object)
        {
            var newId = _cache.Count > 0 ? _cache.Max(x => x.Id) + 1 : 1;
            var newProduct = new Product(newId, product.Name, product.SubtypeId);
            _cache.Add(newProduct);
            return true;
        }
    }

    public Task<bool> DeleteProduct(int id)
    {
        lock (_object)
        {
            var index = _cache.FindIndex(x => x.Id == id);
            bool result = false;
            if (index > -1)
            {
                _cache.RemoveAt(index);
                result = true;
            }

            return Task.FromResult(result);
        }
    }

    public Task<Product?> GetProduct(int id)
    {
        lock (_object)
        {
            var result = _cache.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }
    }

    public Task<Product[]> GetProducts(int subtypeId)
    {
        lock (_object)
        {
            var result = _cache.Where(x => x.SubtypeId == subtypeId).ToArray();
            return Task.FromResult(result);
        }
    }

    public Task<Product[]> GetProducts(int[] ids)
    {
        lock (_object)
        {
            var set = ids.ToHashSet();
            var matches = _cache.Where(x => set.Contains(x.Id)).ToDictionary(x => x.Id, x => x);
            var result = new List<Product>();
            foreach (var id in ids)
                if (matches.ContainsKey(id))
                    result.Add(matches[id]);
            return Task.FromResult(result.ToArray());
        }
    }

    public Task<bool> UpdateProduct(Product product)
    {
        lock (_object)
        {
            var index = _cache.FindIndex(x => x.Id == product.Id);
            bool result = false;
            if (index > -1)
            {
                _cache[index] = product;
                result = true;
            }

            return Task.FromResult(result);
        }
    }
}