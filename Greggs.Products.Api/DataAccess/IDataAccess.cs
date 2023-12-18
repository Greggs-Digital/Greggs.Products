using System.Collections.Generic;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.DataAccess;

public interface IDataAccess<out T> where T : Product
{
    IEnumerable<T> List(int? pageStart, int? pageSize);

    List<Product> GetAll();

}