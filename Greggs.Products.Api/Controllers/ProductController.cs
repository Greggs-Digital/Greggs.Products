namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> Get(int pageStart = 0, int pageSize = 5)
    {
        try
        {
            var products = _productService.GetLatestProducts(pageStart, pageSize);

            //Convert Products from the Model type to a DTO that could be more specific and lightweight
            var response = products.Select(BrandToDto).ToList();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest();
        }
    }

    private static ProductDto BrandToDto(Product product) =>
        new()
        {
            CreatedDate = product.CreatedDate,
            Name = product.Name,
            PriceInPounds = product.PriceInPounds
        };
}