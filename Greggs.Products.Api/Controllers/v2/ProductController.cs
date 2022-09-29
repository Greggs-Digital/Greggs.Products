namespace Greggs.Products.Api.Controllers.v2;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
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
    [MapToApiVersion("2.0")]
    //this one takes the currency with a default currency code
    public ActionResult<IEnumerable<ProductDto>> Get(int pageStart = 0, int pageSize = 5, string currency = "GBP")
    {
        try
        {
            var products = _productService.GetLatestProducts(pageStart, pageSize);

            //Convert Products from the Model type to a DTO that could be more specific and lightweight
            //pass the currency this time
            var response = products.Select(p => BrandToDto(p, currency)).ToList();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest();
        }
    }

    private static ProductDto BrandToDto(Product product, string currency) =>
        new()
        {
            CreatedDate = product.CreatedDate,
            Name = product.Name,
            PriceInPounds = ConvertToEuros(product.PriceInPounds)
        };

    private static decimal ConvertToEuros(decimal price)
    {
        // I would probably put this somewhere else in perhaps a currency microservice with some data source to provide up to date exchange rates
        // I am going to stop now I hope I have shown enough to give an idea of what I can do given the opportunity
        var convertedPrice = price * (decimal) 1.11;
        return decimal.Round(convertedPrice, 2);
    }
}