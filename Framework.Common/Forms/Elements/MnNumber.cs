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
    [FluentValidation.Attributes.Validator(typeof(NumberValidation))]
    [JsonConverter(typeof(MnElementConverter))]
    public class MnNumber : MnTextBox
    {
        public int? MaxLength { get; set; }
        public int? MaxVal { get; set; }
        public int? MinVal { get; set; }

        public MnNumber()
        {
        }

        //[RegularExpression(@"^\d+$", ErrorMessage = "Please enter proper contact details.")]       
        //[Display(Name = "Contact No")]
        //public override string Value
        //{
        //    get
        //    {
        //        return base.Value;
        //    }
        //    set
        //    {
        //        base.Value = value;
        //    }
        //}

        private string _invalidNumberMessage;
        public string InvalidNumberMessage
        {
            get { return _invalidNumberMessage; }
            set { _invalidNumberMessage = value; }
        }
        private string _invalidNumberRangeMessage;
        public string InvalidRangeMessage
        {
            get { return _invalidNumberRangeMessage; }
            set { _invalidNumberRangeMessage = value; }
        }
    }
}
