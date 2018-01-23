using TypelessDatabaseManager.Models;

namespace TypelessDatabaseManager.Helpers
{
    public interface IObjectConverter
    {
        Object ConvertToObject(object obj);
        object ConvertBack(Object obj);
    }
}
