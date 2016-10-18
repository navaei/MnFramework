using System;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{

    #region DatePicker

    [JsonConverter(typeof(MnElementConverter))]
    public class MnDatePicker : MnBaseElement
    {
        public DateTime? Value { get; set; }

        public string HelpText { get; set; }
    }

    #endregion

}
