namespace StoreManagement.WebApp.Controllers;

public class TypeController : AuthorizedController 
{
    private readonly IProductTypeRepository _productTypeRepository;

    public TypeController(IProductTypeRepository productTypeRepository)
    {
        _productTypeRepository = productTypeRepository;
    }

    [HttpGet("/Types")]
    public async Task<IActionResult> Types()
    {
        var types = await _productTypeRepository.GetAll();
        return View(types);
    }
}
