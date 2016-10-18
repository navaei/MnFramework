using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Mn.Framework.Common.Forms.Validation
{
    public class NumberValidation : AbstractValidator<MnNumber>
    {
        public NumberValidation()
        {

            // Required
            RuleFor(x => x.Value)
                .NotEmpty()
                .When(w => w.CurrentMode != AccessMode.Design && w.VisibleMode == VisibilityMode.Both && w.Validations.Any(x => x.Type == ElementValidationType.Required))
                .WithMessage("{0}", x => x.Validations.Single(w => w.Type == ElementValidationType.Required).Message);

            // Only Int
            RuleFor(x => x.Value)
                .Must(MustInt)
                .WithMessage("{0}", x => x.InvalidNumberMessage);

            // Range Validation
            RuleFor(x => x.Value)
                .Must(RangeValidation)
                .When(x => MustInt(x.Value))
                .WithMessage("{0}", x => x.Validations.Single(w => w.Type == ElementValidationType.Range).Message);
        }

        private bool RangeValidation(MnNumber textBox, String value)
        {
            float val;
            // this method not Sensitive to null value if you want do this add requie value
            if (value == null)
            {
                return true;
            }

            // if value not integer 
            if (!float.TryParse(value, out val))
            {
                //set validation message
                textBox.InvalidNumberMessage = "Value must be integer";
                return false;
            }


            //var ElementValidation = (ElementRangeValidation)textBox.Validations.Single(x => x.Type == ElementValidationType.Range);
            //int minValue = textBox.Validations.First(x => x.Type == ElementValidationType.Range).MinValue;
            //int maxValue = textBox.Validations.First(x => x.Type == ElementValidationType.Range).MaxValue;         

            if ((!textBox.MinVal.HasValue || val >= textBox.MinVal) && (!textBox.MaxLength.HasValue || val <= textBox.MaxVal))
            {
                return true; // if in range
            }

            //set validation message
            textBox.InvalidRangeMessage = "Value must between " + textBox.MinVal + " And " + textBox.MaxVal;

            return false; // if not in range

        }

        private bool MustInt(string value)
        {
            // if value could be empty and pass Must intInt Validation
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            float o;
            return float.TryParse(value, out o);
        }
    }
}
