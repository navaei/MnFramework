using System;
using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Serialization;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnCheckBoxList : MnBaseElement, IInputElement
    {
        private string _stringElements;     

        public List<JbSelectedItem> Items { get; set; }

        [JsonIgnore]
        public string JsonItems
        {
            get
            {

                if (Items != null && Items.Any())
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    _stringElements = JsonHelper.JsonSerializer(Items, settings);
                }
                return _stringElements;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _stringElements = value;
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    Items = JsonHelper.JsonDeserialize<List<JbSelectedItem>>(_stringElements, settings);
                }
            }
        }

        public string GetValue()
        {
            return string.Join(",", Items.Where(i => i.Value).ToList());
        }


        public bool IsRequired
        {
            get;
            set;
        }

    }
}