namespace StoreManagement.WebApp;

public class ProductRepository_Tests
{
    [Fact]
    public async Task CreateProduct_MustReturnTrueWhenSubtypeExists()
    {
        // arrange
        const int id = 1;
        var subtype = new ProductSubtype(id, "subtype", id);
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtype(id)).ReturnsAsync(subtype);
        var seed = new List<Product>();
        var repository = new ProductRepository(seed, subtypeRepository.Object);
        var product = new Product(0, "product", id);

        // act
        var result = await repository.CreateProduct(product);

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProduct_MustReturnFalseWhenSubtypeDoesNotExist()
    {
        // arrange
        const int id = 1;
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtype(id)).Returns(Task.FromResult<ProductSubtype?>(null));
        var seed = new List<Product>();
        var repository = new ProductRepository(seed, subtypeRepository.Object);
        var product = new Product(0, "product", id);

        // act
        var result = await repository.CreateProduct(product);

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProduct_MustReturnTrueWhenIdExists()
    {
        // arrange
        const int id = 1;
        var seed = new List<Product>()
        {
            new Product(id, "test", 1)
        };
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        var repository = new ProductRepository(seed, subtypeRepository.Object);

        // act
        var result = await repository.DeleteProduct(id);

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProduct_MustReturnFalseWhenIdDoesNotExist()
    {
        // arrange
        const int id = 1;
        var seed = new List<Product>();
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        var repository = new ProductRepository(seed, subtypeRepository.Object);

        // act
        var result = await repository.DeleteProduct(id);

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateProduct_MustReturnTrueWhenIdExists()
    {
        // arrange
        const int id = 1;
        var product = new Product(id, "test prod", id);
        var seed = new List<Product>()
        {
            product
        };
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        var repository = new ProductRepository(seed, subtypeRepository.Object);

        // act
        var result = await repository.UpdateProduct(product);

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateProduct_MustReturnFalseWhenIdDoesNotExist()
    {
        // arrange
        const int id = 1;
        var product = new Product(id, "test prod", id);
        var seed = new List<Product>();
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        var repository = new ProductRepository(seed, subtypeRepository.Object);

        // act
        var result = await repository.UpdateProduct(product);

        // assert
        Assert.False(result);
    }
}