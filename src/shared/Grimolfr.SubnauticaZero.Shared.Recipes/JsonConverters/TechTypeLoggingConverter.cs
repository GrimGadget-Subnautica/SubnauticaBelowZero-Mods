using System;
using Newtonsoft.Json;

namespace Grimolfr.SubnauticaZero.JsonConverters
{
    public class TechTypeLoggingConverter
        : JsonConverter<TechType>
    {
        public override void WriteJson(JsonWriter writer, TechType value, JsonSerializer serializer)
        {
            writer.WriteValue(value.AsString());
        }

        public override TechType ReadJson(JsonReader reader, Type objectType, TechType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
