using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mn.Framework.Common.Forms.Validation
{
    public enum ElementValidationType
    {
        None,
        Required,
        Range,
        Numeric,
        MaxLength,
        Email
    }

    public class BaseElementValidator
    {
        public BaseElementValidator()
        {

        }

        public BaseElementValidator(ElementValidationType type)
        {
            SeedElementValidator(type);
        }

        private void SeedElementValidator(ElementValidationType type)
        {
            Type = type;
            switch (type)
            {
                case ElementValidationType.Numeric:
                    {
                        Message = "You must enter an Integer!";
                        break;
                    }
                case ElementValidationType.Required:
                    {
                        Message = "The filed can not be empty!";
                        break;
                    }
                case ElementValidationType.Email:
                    {
                        Message = "This e-mail address is not valid";
                        break;
                    }

            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ElementValidationType Type { get; set; }

        [DisplayName("Error message")]
        public string Message { get; set; }

        public bool IsChecked { get; set; }
    }
}
