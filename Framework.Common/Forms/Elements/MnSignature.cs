using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Common.Forms.Validation;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    [FluentValidation.Attributes.Validator(typeof(SignatureValidator))]
    public class MnSignature : MnBaseElement
    {
        public string Value { get; set; }

        public string HelpText { get; set; }

        //[ForceTrue(ErrorMessage = "Signature is required field.")]
        public bool HasSigned { get; set; }
    }
}
