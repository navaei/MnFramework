using System;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnTextArea : MnBaseElement, IInputElement
    {
        public bool IsChecked { get; set; }

        public string Value
        {
            get;
            set;
        }

        public string GetValue()
        {
            return Value;
        }


        public bool IsRequired
        {
            get;
            set;
        }

        public string PlaceHolder { get; set; }

    }
}