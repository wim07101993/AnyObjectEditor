using Newtonsoft.Json;

namespace Shared.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T This) 
            => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(This));
    }
}
