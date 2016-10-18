using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnHidden : MnBaseElement
    {
        public string Value { get; set; }

        public string GetValue()
        {
            return Value;
        }      
    }
}
