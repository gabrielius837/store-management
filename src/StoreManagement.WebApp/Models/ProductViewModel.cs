
namespace StoreManagement.WebApp;

public class ProductViewModel
{
    public int Id { get; set; } = default!;
    [Required]
    public string Name { get; set; } = default!;
    public int SubtypeId { get; set; } = default!;
    public IEnumerable<SelectListItem> SelectListItems { get; set; } = default!;
}