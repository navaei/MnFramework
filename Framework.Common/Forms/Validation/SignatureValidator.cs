using FluentValidation;

namespace Mn.Framework.Common.Forms.Validation
{
    public class SignatureValidator : AbstractValidator<MnSignature>
    {
        public SignatureValidator()
        {
            // Required always would be checked for the sign
            //RuleFor(x => x.HasSigned)
            //    .NotEqual(false);
        }
    }
}