using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Greggs.Products.Api.Mediator;

public class GetProductHandler : IRequestHandler<GetProductRequest, IEnumerable<Product>>
{
    private readonly IDataAccess<Product> _dataAccessService;

    public GetProductHandler(IDataAccess<Product> dataAccessService)
    {
        _dataAccessService = dataAccessService;
    }
    
    public async Task<IEnumerable<Product>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dataAccessService.List(request.PageStart, request.PageSize, request.DefaultCurrency));
    }
}