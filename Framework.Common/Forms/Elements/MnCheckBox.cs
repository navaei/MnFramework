using System;
using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Serialization;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnCheckBox : MnBaseElement, IInputElement
    {
        private string _stringElements;

        public bool Value
        {
            get;
            set;
        }        

        public string GetValue()
        {
            return Value.ToString();
        }


        public bool IsRequired
        {
            get;
            set;
        }
        public string PlaceHolder { get; set; }

    }
}