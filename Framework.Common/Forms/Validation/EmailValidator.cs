using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;

namespace Mn.Framework.Common.Forms.Validation
{
    public class EmailValidator : AbstractValidator<MnEmail>
    {
        public EmailValidator()
        {
            // Range Validation
            RuleFor(x => x.Value)
                .Must(EmailValidation)
                .WithMessage("{0}", x => x.InvalidEmailMessage);

            // Required
            RuleFor(x => x.Value)
                .NotEmpty()
                .When(w => w.CurrentMode != AccessMode.Design && w.VisibleMode == VisibilityMode.Both && w.Validations.Any(x => x.Type == ElementValidationType.Required))
                .WithMessage("{0}", x => x.Validations.Single(w => w.Type == ElementValidationType.Required).Message);
        }

        private bool EmailValidation(MnEmail textBox, String emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return true;
            return Regex.IsMatch(emailAddress, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
