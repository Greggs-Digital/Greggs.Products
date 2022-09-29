namespace Greggs.Products.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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
    [MapToApiVersion("1.0")]
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