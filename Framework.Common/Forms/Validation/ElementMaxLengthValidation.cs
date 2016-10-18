using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mn.Framework.Common.Forms.Validation
{
    [Serializable]
    public class ElementMaxLengthValidation : BaseElementValidator, IValidatableObject
    {
        public ElementMaxLengthValidation()
        {
            Type = ElementValidationType.MaxLength;
            Message = "The value you entered exceed maximum lentgth";
        }

        [Required]
        [DisplayName("Maximum length")]
        public int Length { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var list = new List<ValidationResult>();
            if (IsChecked)
            {
                if (Length <= 0)
                {
                    list.Add(new ValidationResult("Maximum length can not be negative or zero ", new[] { "Length" }));
                }
            }
            return list.AsEnumerable();
        }


    }
}