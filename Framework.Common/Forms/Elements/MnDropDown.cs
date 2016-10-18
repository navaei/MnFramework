using System;
using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Serialization;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnDropDown : MnBaseElement, IInputElement
    {
        private string _stringElements;
        public string DefaultText { get; set; }
        public List<MnElementItem> Items { get; set; }

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
                    Items = JsonHelper.JsonDeserialize<List<MnElementItem>>(_stringElements, settings);
                }
            }
        }


        public string Value
        {
            get;
            set;
        }

        public override object GetValue()
        {
            return Items.Any() && Items.Any(i => i.Value == Value) ? Items.Single(i => i.Value == Value).Text : null;
        }


        public bool IsRequired
        {
            get;
            set;
        }

        public bool EqualsItems(MnDropDown destDropDown)
        {
            if (Items.Count != destDropDown.Items.Count)
                return false;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Value != destDropDown.Items[i].Value || Items[i].Text != destDropDown.Items[i].Text)
                    return false;
            }
            return true;
        }

    }
}