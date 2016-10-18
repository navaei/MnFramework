using System;
using System.Linq;
using FluentValidation;

namespace Mn.Framework.Common.Forms.Validation
{
    public class TextBoxValidator : AbstractValidator<MnTextBox>
    {
        public TextBoxValidator()
        {

            // Required
            RuleFor(x => x.Value)
                .NotEmpty()
                .When(w => w.CurrentMode != AccessMode.Design && w.VisibleMode == VisibilityMode.Both && w.Validations.Any(x => x.Type == ElementValidationType.Required))
                .WithMessage("{0}", x => x.Validations.Single(w => w.Type == ElementValidationType.Required).Message);

        }

    }
}