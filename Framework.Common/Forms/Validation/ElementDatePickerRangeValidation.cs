using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mn.Framework.Common.Forms.Validation
{
    [Serializable]
    public class ElementDatePickerRangeValidation : BaseElementValidator, IValidatableObject
    {
        public ElementDatePickerRangeValidation()
        {
            Type = ElementValidationType.Range;
            Message = "Not valid date in range";
            MinValue = DateTime.Now.AddDays(-15);
            MaxValue = DateTime.Now.AddDays(15);
        }

        [Required]
        public DateTime MinValue { get; set; }

        [Required]
        public DateTime MaxValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var list = new List<ValidationResult>();
            if (IsChecked)
            {
                if (MaxValue <= MinValue)
                {
                    list.Add(new ValidationResult("Maximum value can not equal or less than Minimum value", new[] { "MaxValue" }));
                }
            }
            return list.AsEnumerable();
        }
    }
}