namespace StoreManagement.WebApp.Controllers;

public class ProductController : AuthorizedController
{
    private readonly IProductRepository _productRepository;
    private readonly IProductSubtypeRepository _subtypeRepository;
    private readonly IProductCookieManager _manager;

    public ProductController(IProductRepository productRepository, IProductSubtypeRepository subtypeRepository, IProductCookieManager manager)
    {
        _productRepository = productRepository;
        _subtypeRepository = subtypeRepository;
        _manager = manager;
    }

    [HttpGet("/Products/{id}")]
    public async Task<IActionResult> Products(int id)
    {
        var products = await _productRepository.GetProducts(id);
        return View(products);
    }

    [HttpGet("/Product/{id}")]
    public async Task<IActionResult> Product(int id)
    {
        var subtypes = await _subtypeRepository.GetSubtypes();

        if (id == 0)
        {
            var subtype = subtypes.FirstOrDefault();
            var sid = subtype?.Id ?? 0;
            var items = subtypes.Select(x => new SelectListItem(x.Name, x.Id.ToString(), x.Id == sid));
            var viewModel = new ProductViewModel()
            {
                SubtypeId = sid,
                SelectListItems = items
            };
            return View(viewModel);
        }

        var product = await _productRepository.GetProduct(id);
        if (product is null)
        {
            return View("NotFound");
        }
        else
        {
            _manager.UpdateProductHistoryCookie(HttpContext, id);
            var items = subtypes.Select(x => new SelectListItem(x.Name, x.Id.ToString(), x.Id == product.SubtypeId));
            var viewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                SubtypeId = product.SubtypeId,
                SelectListItems = items
            };
            return View(viewModel);
        }
    }

    [HttpGet("/History")]
    public async Task<IActionResult> History()
    {
        var ids = _manager.ParseProductHistoryCookie(HttpContext);
        var products = await _productRepository.GetProducts(ids);
        return View("Products", products);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("/Product/Upsert")]
    public async Task<IActionResult> Upsert(ProductViewModel viewModel)
    {
        const string key = "SelectListItems";
        ModelState.Remove(key);

        var subtypes = await _subtypeRepository.GetSubtypes();
        var items = subtypes.Select(subtype => new SelectListItem(subtype.Name, subtype.Id.ToString(), subtype.Id == viewModel.SubtypeId));
        viewModel.SelectListItems = items;
        var product = new Product(viewModel.Id, viewModel.Name, viewModel.SubtypeId);

        if (ModelState.ErrorCount == 0 && product.Id == 0 && await _productRepository.CreateProduct(product))
            return RedirectToAction("Products", new { id = product.SubtypeId });

        if (ModelState.ErrorCount == 0 && product.Id > 0 && (await _productRepository.UpdateProduct(product)))
            return RedirectToAction("Products", new { id = product.SubtypeId });

        return View("Product", viewModel);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("/Product/Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetProduct(id);
        var result = product is not null;
        if (product is not null)
        {
            await _productRepository.DeleteProduct(id);
            return RedirectToAction("Products", "Product", new { id = product.SubtypeId });
        }
        else
            return RedirectToAction("Types", "Type");
    }
}