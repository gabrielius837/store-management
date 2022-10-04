namespace StoreManagement.WebApp;

public static class SeedFactory
{
    private static List<ProductType> TypeSeed()
    {
        return new List<ProductType>()
        {
            new ProductType(1, "Food"),
            new ProductType(2, "Electronics")
        };
    }

    private static List<ProductSubtype> SubtypeSeed()
    {
        return new List<ProductSubtype>()
        {
            new ProductSubtype(1, "Milk", 1),
            new ProductSubtype(2, "Bread", 1),
            new ProductSubtype(3, "Tv", 2),
            new ProductSubtype(4, "Computer", 2),
        };
    }

    private static List<Product> ProductSeed()
    {
        return new List<Product>()
        {
            new Product(1, "Cow milk", 1),
            new Product(2, "Goat milk", 1),
            new Product(3, "Rye bread", 2),
            new Product(4, "Sourdough bread", 2),
            new Product(5, "Tv one", 3),
            new Product(6, "Tv two", 3),
            new Product(7, "Macbook", 4),
            new Product(8, "Laptop", 4),
        };
    }

    public static IServiceCollection RegisterDataServices(this IServiceCollection services)
    {
        var types = TypeSeed();
        var subtypes = SubtypeSeed();
        var products = ProductSeed();
        var productTypeRepository = new ProductTypeRepository(types);
        var productSubtypeRepository = new ProductSubtypeRepository(subtypes);
        var productRepository = new ProductRepository(products, productSubtypeRepository);

        services.AddSingleton<IProductTypeRepository>(productTypeRepository);
        services.AddSingleton<IProductSubtypeRepository>(productSubtypeRepository);
        services.AddSingleton<IProductRepository>(productRepository);
        
        return services;
    }
}