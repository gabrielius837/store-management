
namespace StoreManagement.UnitTests;

public class ProductController_Tests
{
    [Fact]
    public async Task Product_MustReturnProductView()
    {
        // arrange
        const int id = 1;
        ProductSubtype[] subtypes = { new ProductSubtype(id, "test sub", id) };
        var product = new Product(id, "test prod", id);
        var productRepository = new Mock<IProductRepository>();
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtypes()).ReturnsAsync(subtypes);
        productRepository.Setup(x => x.GetProduct(id)).ReturnsAsync(product);
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.Product(id);
        var viewResult = result as ViewResult;

        // assert
        Assert.NotNull(viewResult);
        Assert.NotNull(viewResult?.Model);
        Assert.Equal((viewResult?.Model as ProductViewModel)?.Id, id);
    }

    [Fact]
    public async Task Product_MustReturnNotFoundView()
    {
        // arrange
        const int id = 1;
        ProductSubtype[] subtypes = { new ProductSubtype(id, "test sub", id) };
        var product = new Product(id, "test prod", id);
        var productRepository = new Mock<IProductRepository>();
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtypes()).ReturnsAsync(subtypes);
        productRepository.Setup(x => x.GetProduct(id)).ReturnsAsync(product);
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.Product(2);
        var viewResult = result as ViewResult;

        // assert
        Assert.NotNull(viewResult);
        Assert.Equal("NotFound", viewResult?.ViewName);
        Assert.Null(viewResult?.Model);
    }


    [Fact]
    public async Task History_MustHaveHistory()
    {
        // arrange
        const int id = 5;
        int[] ids = { id };
        Product[] products = { new Product(id, "test prod", id) };
        var productRepository = new Mock<IProductRepository>();
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        productRepository.Setup(x => x.GetProducts(ids)).ReturnsAsync(products);
        var manager = new Mock<IProductCookieManager>();
        manager.Setup(x => x.ParseProductHistoryCookie(It.IsAny<HttpContext>())).Returns(ids);
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.History();
        var viewResult = result as ViewResult;

        // assert
        Assert.NotNull(viewResult);
        Assert.Equal(products, viewResult?.Model);
    }

    [Fact]
    public async Task Upsert_MustCreateProduct()
    {
        // arrange
        var flag = false;
        const int id = 1;
        const int zero = 0;
        int[] ids = { id };
        var viewModel = new ProductViewModel()
        {
            Id = zero,
            Name = "test prod",
            SubtypeId = id
        };
        ProductSubtype[] subtypes = { new ProductSubtype(id, "test sub", id) };
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.CreateProduct(It.IsAny<Product>())).ReturnsAsync(true).Callback(() => flag = true);
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtypes()).ReturnsAsync(subtypes);
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.Upsert(viewModel);
        var redirectResult = result as RedirectToActionResult;

        // assert
        Assert.NotNull(redirectResult);
        Assert.True(flag);
        Assert.Equal("Products", redirectResult?.ActionName);
    }

    [Fact]
    public async Task Upsert_MustUpdateProduct()
    {
        // arrange
        var flag = false;
        const int id = 1;
        int[] ids = { id };
        var viewModel = new ProductViewModel()
        {
            Id = id,
            Name = "test prod",
            SubtypeId = id
        };
        ProductSubtype[] subtypes = { new ProductSubtype(id, "test sub", id) };
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.CreateProduct(It.IsAny<Product>())).ReturnsAsync(false);
        productRepository.Setup(x => x.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(true).Callback(() => flag = true);
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtypes()).ReturnsAsync(subtypes);
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.Upsert(viewModel);
        var redirectResult = result as RedirectToActionResult;

        // assert
        Assert.NotNull(redirectResult);
        Assert.True(flag);
        Assert.Equal("Products", redirectResult?.ActionName);
    }

    [Fact]
    public async Task Upsert_MustFail()
    {
        // arrange
        const int id = 1;
        int[] ids = { id };
        var viewModel = new ProductViewModel()
        {
            Id = id,
            Name = "test prod",
            SubtypeId = id
        };
        ProductSubtype[] subtypes = { new ProductSubtype(id, "test sub", id) };
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.CreateProduct(It.IsAny<Product>())).ReturnsAsync(false);
        productRepository.Setup(x => x.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(false);
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        subtypeRepository.Setup(x => x.GetSubtypes()).ReturnsAsync(subtypes);
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);
        var metadata = new Mock<ModelMetadata>();
        controller.ModelState.TryAddModelException("test", new System.Exception());

        // act
        var result = await controller.Upsert(viewModel);
        var viewResult = result as ViewResult;

        // assert
        Assert.NotNull(viewResult);
        Assert.Equal("Product", viewResult?.ViewName);
        Assert.Equal(viewModel, viewResult?.Model);
    }


    [Fact]
    public async Task Upsert_MustDelete()
    {
        // arrange
        const int id = 1;
        var product = new Product(id, "test prod", id);
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.GetProduct(id)).ReturnsAsync(product);
        productRepository.Setup(x => x.DeleteProduct(id)).ReturnsAsync(true);
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.Delete(id);
        var redirectResult = result as RedirectToActionResult;

        // assert
        Assert.NotNull(redirectResult);
        Assert.Equal("Products", redirectResult?.ActionName);
        Assert.Equal("Product", redirectResult?.ControllerName);
    }

    [Fact]
    public async Task Upsert_MustNotDelete()
    {
        // arrange
        const int id = 1;
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.GetProduct(id)).Returns(Task.FromResult<Product?>(null));
        var subtypeRepository = new Mock<IProductSubtypeRepository>();
        var manager = new Mock<IProductCookieManager>();
        var controller = new ProductController(productRepository.Object, subtypeRepository.Object, manager.Object);

        // act
        var result = await controller.Delete(id);
        var redirectResult = result as RedirectToActionResult;

        // assert
        Assert.NotNull(redirectResult);
        Assert.Equal("Types", redirectResult?.ActionName);
        Assert.Equal("Type", redirectResult?.ControllerName);
    }
}