using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    public class CreateEditCheckBoxViewModel : MnBaseElementViewModel
    {
        //public bool IsRequired { get; set; }

        public CreateEditCheckBoxViewModel()
        {
            // check box has no validation
            SeedSupportedValidation();
        }

        public CreateEditCheckBoxViewModel(MnCheckBox datePicker)
        {
            Title = datePicker.Title;
            HelpText = datePicker.HelpText;
        }

        private void SeedSupportedValidation()
        {
            var validationList = new List<BaseElementValidator>
                {
                    new BaseElementValidator(ElementValidationType.None){IsChecked = true}
                };

            ElementValidators.AddRange(validationList);
        }

        public override object ToObject()
        {
            // create checkBox from CreateEditCheckBoxViewModel
            var validation = new List<BaseElementValidator>();

            validation.AddRange(ElementValidators.Where(x => x.IsChecked).ToList());

            var obj = new MnCheckBox() { Title = Title, HelpText = HelpText };
            obj.Validations.AddRange(validation);
            return obj;
        }
    }
}