using System;
using Newtonsoft.Json;

namespace IreckonuShop.Common.Utilities.Serialization
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public T Deserialize(string serialization)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(serialization);
                return obj;
            }
            catch (JsonException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public string Serialize(T item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}