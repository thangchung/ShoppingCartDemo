using Newtonsoft.Json;

namespace NT.Infrastructure
{
    public static class ObjectConvertExtensions
    {
        public static TObject ToObject<TObject>(this string source)
            where TObject : class
        {
            return JsonConvert.DeserializeObject<TObject>(source);
        }

        public static string ToString<TObject>(this TObject source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}