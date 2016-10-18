using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    #region DatePicker   

    [Serializable]
    public class DatePicker : MnBaseElement
    {
        public DateTime? Value { get; set; }

        public string HelpText { get; set; }
    }

    #endregion
}
