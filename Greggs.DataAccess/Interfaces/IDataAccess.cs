namespace Greggs.DataAccess.Interfaces;

public interface IDataAccess<out T>
{
    IEnumerable<T> List(int? pageStart, int? pageSize);

    IEnumerable<T> ListOrderByDescending(int? pageStart, int? pageSize, string param);
}