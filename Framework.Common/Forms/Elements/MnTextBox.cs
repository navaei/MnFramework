using System;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Common.Forms.Validation;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [FluentValidation.Attributes.Validator(typeof(TextBoxValidator))]
    [JsonConverter(typeof(MnElementConverter))]
    public class MnTextBox : MnBaseElement, IInputElement
    {

        public virtual string Value
        {

            get;
            set;
        }

        public override object GetValue()
        {
            return Value == null ? string.Empty : Value;
        }


        public bool IsRequired { get; set; }


        public string PlaceHolder { get; set; }
    }
}