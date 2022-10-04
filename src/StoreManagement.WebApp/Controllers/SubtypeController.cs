namespace StoreManagement.WebApp.Controllers;

public class SubtypeController : AuthorizedController 
{
    private readonly IProductSubtypeRepository _productSubtypeRepository;

    public SubtypeController(IProductSubtypeRepository productSubtypeRepository)
    {
        _productSubtypeRepository = productSubtypeRepository;
    }

    [HttpGet("/Subtype/{id}")]
    public async Task<IActionResult> Subtypes(int id)
    {
        var result = await _productSubtypeRepository.GetChildren(id);
        return View(result);
    }
}
