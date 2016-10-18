using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnParagraph : MnBaseElement, IInputElement
    {
        public virtual string Value
        {
            get;
            set;
        }

        public string GetValue()
        {
            return Value;
        }
    }
}