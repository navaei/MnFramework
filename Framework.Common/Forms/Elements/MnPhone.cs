using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnPhone : MnTextBox
    {

        [DisplayName("Phone type")]
        public PhoneType PhoneType { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^(\([0-9]{3}\)|[0-9]{3}-)[0-9]{3}-[0-9]{4}$|^[0-9]*$", ErrorMessage = "Invalid phone number")]
        public override string Value { get; set; }
    }

    public enum PhoneType : byte
    {
        [Description("US phone number")]
        US = 0,
        [Description("International phone number")]
        INTERNATIONAL = 1
    }
}
