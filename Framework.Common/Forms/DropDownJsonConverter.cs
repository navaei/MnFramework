using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mn.Framework.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mn.Framework.Common.Forms
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            JsonHelper.JsonSerializer(value);
        }
    }

    public class DropDownJsonConverter : JsonConverter
    {
        

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var ddr = new MnDropDown();
            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), ddr);
            return ddr;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Serialize(writer, value);
        }
    }
}
