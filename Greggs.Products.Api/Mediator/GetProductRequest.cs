using Greggs.Products.Api.Models;
using MediatR;
using System.Collections.Generic;

namespace Greggs.Products.Api.Mediator;

public class GetProductRequest : IRequest<IEnumerable<Product>>
{
    public int PageStart { get; set; }
    public int PageSize { get; set; }
    public string DefaultCurrency { get; set; }
}