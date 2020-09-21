using System;
using System.Collections.Generic;

namespace Greggs.Products.Api.Respositories
{
    public interface IDataRepository<out T>
    {
        /// <summary>
        /// Returns a list of products
        /// </summary>
        /// <param name="pageStart">Index of the first item to be returned</param>
        /// <param name="pageSize">Number of items to be returned</param>
        /// <returns></returns>
        IEnumerable<T> List(int? pageStart, int? pageSize);
    }
}