using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Common.Forms.Validation;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [FluentValidation.Attributes.Validator(typeof(EmailValidator))]
    [JsonConverter(typeof(MnElementConverter))]
    public class MnEmail : MnTextBox
    {
        public MnEmail()
        {
            _invalidEmailMessage = "Invalid email address.";
        }

        private string _invalidEmailMessage;
        public string InvalidEmailMessage
        {
            get { return _invalidEmailMessage; }
            set { _invalidEmailMessage = value; }
        }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }        
    }
}
