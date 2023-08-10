using System.Collections.Generic;

namespace Greggs.Products.Abstractions;

public interface IDataAccess<out T>
{
    IEnumerable<T> List(int? pageStart, int? pageSize);
    IEnumerable<T> List(int? pageStart, int? pageSize, string connectionString);


}