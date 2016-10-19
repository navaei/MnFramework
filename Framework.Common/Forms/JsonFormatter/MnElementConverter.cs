using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Mn.Framework.Common.Forms.JsonFormatter
{
    public class MnElementConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IMnBaseElement element = value as MnBaseElement ?? value as IMnBaseElement;
            var properties = element.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)));

            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            serializer.Serialize(writer, element.GetType().Name);

            if (this.GetType().Assembly.GetName().Name != element.GetType().Assembly.GetName().Name)
            {
                writer.WritePropertyName("Type.FullName");
                serializer.Serialize(writer, element.GetType().FullName);
                writer.WritePropertyName("Type.AssemblyName");
                serializer.Serialize(writer, element.GetType().Assembly.GetName().Name);
            }

            writer.WritePropertyName("IsCustomElement");
            serializer.Serialize(writer, element is ICustomElement ? true : false);

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(JsonConverterAttribute)))
                {
                    var jsonAttr = property.GetCustomAttribute<JsonConverterAttribute>(false);
                    if (jsonAttr.ConverterType == typeof(StringEnumConverter))
                    {
                        writer.WritePropertyName(property.Name);
                        serializer.Serialize(writer, Enum.ToObject(property.PropertyType, property.GetValue(element)).ToString());
                    }
                }
                else
                {
                    writer.WritePropertyName(property.Name);
                    serializer.Serialize(writer, property.GetValue(element));
                }
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            return GetJbElement(jsonObject);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(MnBaseElement).IsAssignableFrom(objectType);
        }

        #region private

        internal IMnBaseElement GetJbElement(JObject jsonObject)
        {
            IMnBaseElement mnElement;
            var properties = jsonObject.Properties().ToList();
            if (properties.All(p => p.Name != "Type"))
                return null;
            var elementType = (string)properties.Single(p => p.Name == "Type").Value;
            var typeFull = properties.SingleOrDefault(p => p.Name == "Type.FullName");
            var assemblyName = properties.SingleOrDefault(p => p.Name == "Type.AssemblyName");
            if (elementType == typeof(MnTextBox).Name)
            {
                mnElement = new MnTextBox();
                (mnElement as MnTextBox).Value = (string)properties.Single(p => p.Name == "Value").Value;
                if (properties.Any(p => p.Name == "PlaceHolder"))
                    (mnElement as MnTextBox).PlaceHolder = (string)properties.Single(p => p.Name == "PlaceHolder").Value;
                else
                    (mnElement as MnTextBox).PlaceHolder = string.Empty;
            }
            else if (elementType == typeof(MnNumber).Name)
            {
                mnElement = new MnNumber();
                (mnElement as MnNumber).Value = (string)properties.Single(p => p.Name == "Value").Value;
                if (properties.Any(p => p.Name == "PlaceHolder"))
                    (mnElement as MnNumber).PlaceHolder = (string)properties.Single(p => p.Name == "PlaceHolder").Value;
                else
                    (mnElement as MnNumber).PlaceHolder = string.Empty;

                if (properties.Any(p => p.Name == "MinVal") && !string.IsNullOrEmpty((string)properties.Single(p => p.Name == "MinVal").Value))
                    (mnElement as MnNumber).MinVal = (int)properties.Single(p => p.Name == "MinVal").Value;

                if (properties.Any(p => p.Name == "MaxVal") && !string.IsNullOrEmpty((string)properties.Single(p => p.Name == "MaxVal").Value))
                    (mnElement as MnNumber).MaxVal = (int)properties.Single(p => p.Name == "MaxVal").Value;
            }
            else if (elementType == typeof(MnEmail).Name)
            {
                mnElement = new MnEmail();
                (mnElement as MnEmail).Value = (string)properties.Single(p => p.Name == "Value").Value;
                if (properties.Any(p => p.Name == "PlaceHolder"))
                    (mnElement as MnEmail).PlaceHolder = (string)properties.Single(p => p.Name == "PlaceHolder").Value;
                else
                    (mnElement as MnEmail).PlaceHolder = string.Empty;
            }
            else if (elementType == typeof(MnParagraph).Name)
            {
                mnElement = new MnParagraph();
                (mnElement as MnParagraph).Value = (string)properties.Single(p => p.Name == "Value").Value;
            }
            else if (elementType == typeof(MnHidden).Name)
            {
                mnElement = new MnHidden();
                (mnElement as MnHidden).Value = (string)properties.Single(p => p.Name == "Value").Value;
            }
            else if (elementType == typeof(MnCheckBox).Name)
            {
                mnElement = new MnCheckBox();
                (mnElement as MnCheckBox).Value = (bool)properties.Single(p => p.Name == "Value").Value;
            }
            else if (elementType == typeof(MnDropDown).Name)
            {
                mnElement = new MnDropDown();
                (mnElement as MnDropDown).Items =
                    properties.Single(p => p.Name == "Items").Value.ToObject<List<MnElementItem>>();

            }
            else if (elementType == typeof(MnCheckBoxList).Name)
            {
                mnElement = new MnCheckBoxList();
                (mnElement as MnCheckBoxList).Items =
                      properties.Single(p => p.Name == "Items").Value.ToObject<List<JbSelectedItem>>();

            }
            else if (elementType == typeof(MnSection).Name)
            {
                mnElement = new MnSection();
                var innerElements = properties.Single(p => p.Name == "Elements").Value.ToObject<List<object>>();
                foreach (var element in innerElements)
                {
                    (mnElement as MnSection).Elements.Add(GetJbElement(element as JObject));
                }
            }
            else if (elementType == typeof(MnAddress).Name)
            {
                mnElement = new MnAddress();
            }
            else
            {
                Type typ;
                if (typeFull == null)
                    typ = Type.GetType(string.Format("{0}.Forms.{1}, {0}", this.GetType().Assembly.GetName().Name, elementType), true);
                else
                    typ = Type.GetType((string)typeFull.Value + "," + (string)assemblyName.Value, true);

                mnElement = (IMnBaseElement)Activator.CreateInstance(typ);
            }

            if (mnElement is IInputElement)
            {
                //if (properties.Any(p => p.Name == "IsRequired"))
                //    (mnElement as IInputElement).IsRequired =
                //        (bool)properties.Single(p => p.Name == "IsRequired").Value;
                //else
                //    (mnElement as IInputElement).IsRequired = false;
            }

            foreach (var property in properties)
            {
                if (property.Name.Equals("Type"))
                    continue;
                var proInfo = mnElement.GetType().GetProperty(property.Name);
                if (proInfo == null)
                    continue;
                if (proInfo.PropertyType == mnElement.AccessRole.GetType())
                {
                    var accessRoles = property.Value.ToObject<Dictionary<string, AccessMode>>();
                    proInfo.SetValue(mnElement, accessRoles);
                }
                else if (proInfo.PropertyType == mnElement.Validations.GetType())
                {
                    var validations = property.Value.ToObject<List<BaseElementValidator>>();
                    proInfo.SetValue(mnElement, validations);
                }
                else if (Attribute.IsDefined(proInfo, typeof(JsonConverterAttribute)))
                {
                    var jsonAttr = proInfo.GetCustomAttribute<JsonConverterAttribute>(false);
                    if (jsonAttr.ConverterType == typeof(StringEnumConverter))
                    {
                        var mode = property.Value.ToObject(proInfo.PropertyType);
                        proInfo.SetValue(mnElement, Enum.ToObject(proInfo.PropertyType, mode));
                    }

                }
                else
                {
                    if (typeof(System.Collections.IList).IsAssignableFrom(proInfo.PropertyType))
                        continue;
                    if (!string.IsNullOrEmpty((string)property.Value))
                        proInfo.SetValue(mnElement, Convert.ChangeType(property.Value, proInfo.PropertyType));
                }

            }
            return mnElement as MnBaseElement ?? mnElement as IMnBaseElement;
        }

        #endregion
    }
}
