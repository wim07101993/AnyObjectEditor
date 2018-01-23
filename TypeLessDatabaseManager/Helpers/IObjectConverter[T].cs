using TypelessDatabaseManager.Models;

namespace TypelessDatabaseManager.Helpers
{
    public interface IObjectConverter<T>
    {
        Object ConvertToObject(T obj);
        T ConvertBack(Object obj);
    }
}
