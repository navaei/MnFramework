using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mn.Framework.Common.Forms.Binding;

namespace Mn.Framework.Common.Forms.JsonFormatter
{
    public class MnFormConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(MnForm).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            MnForm form = new MnForm();
            JObject jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();
            foreach (var property in properties)
            {
                if (property.Name.Equals("Type"))
                    continue;
                var proInfo = form.GetType().GetProperty(property.Name);

                if (proInfo.PropertyType == form.Elements.GetType())
                {
                    var jbc = new MnElementConverter();
                    var props = property.Value.ToObject<List<object>>();
                    List<IMnBaseElement> elements = props.Select(prop => jbc.GetJbElement(prop as JObject)).ToList();

                    proInfo.SetValue(form, elements);
                }
                else if (proInfo.PropertyType == form.AccessRole.GetType())
                {
                    var accessRoles = property.Value.ToObject<Dictionary<string, AccessMode>>();
                    proInfo.SetValue(form, accessRoles);
                }
                else
                {
                    //if (typeof(System.Collections.IEnumerable).IsAssignableFrom(proInfo.PropertyType))
                    //    continue;
                    if (proInfo.PropertyType != typeof(BindingConfig))
                        proInfo.SetValue(form, Convert.ChangeType(property.Value, proInfo.PropertyType));
                }

            }
            return form;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var form = value as MnForm;
            writer.WriteStartObject();
            var properties = form.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)));

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(JsonIgnoreAttribute)))
                    continue;
                writer.WritePropertyName(property.Name);
                serializer.Serialize(writer, property.GetValue(form));
            }

            writer.WriteEndObject();
        }
    }
}
