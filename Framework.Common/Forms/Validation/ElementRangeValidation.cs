using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mn.Framework.Common.Forms.Validation
{
    [Serializable]
    public class ElementRangeValidation : BaseElementValidator, IValidatableObject
    {
        public ElementRangeValidation()
        {
            Type = ElementValidationType.Range;
            Message = "Not valid number in range";
        }

        [Required]
        public int MinValue { get; set; }

        [Required]
        public int MaxValue { get; set; }

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