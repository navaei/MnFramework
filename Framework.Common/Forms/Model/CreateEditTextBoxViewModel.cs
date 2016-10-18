using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    public class CreateEditTextBoxViewModel : MnBaseElementViewModel
    {
        public bool IsRequired { get; set; }

        public CreateEditTextBoxViewModel()
        {
            // load supported validation
            SeedSupportedValidation();
        }

        public CreateEditTextBoxViewModel(MnTextBox textBox)
        {
            Title = textBox.Title;
            HelpText = textBox.HelpText;
            IsRequired = textBox.Validations.Any(x => x.Type == ElementValidationType.Required);
            SelectedValidationId = (int)textBox.Validations.First(x => x.Type != ElementValidationType.Required).Type;

            // file ElementValidators by default validation and default value
            SeedSupportedValidation();

            // if my textBox has validation replace it validation by default ElementValidators
            foreach (var item in textBox.Validations.Where(x => x.Type != ElementValidationType.Required))
            {
                var replacedValidator = ElementValidators.Single(x => x.Type == item.Type);
                var index = ElementValidators.IndexOf(replacedValidator);
                replacedValidator = item;
                ElementValidators.RemoveAt(index);
                ElementValidators.Insert(index, replacedValidator);
            }


        }

        private void SeedSupportedValidation()
        {
            var validationList = new List<BaseElementValidator>
                {
                    new BaseElementValidator(ElementValidationType.None),
                    new BaseElementValidator(ElementValidationType.Numeric),
                    new ElementRangeValidation(),
                    new ElementMaxLengthValidation(),
                    new BaseElementValidator(ElementValidationType.Email)
                };

            ElementValidators.AddRange(validationList);
            Validations = validationList.Select(p => new MnElementItem
                {
                    Value = ((int)(p.Type)).ToString(),
                    Text = p.Type.ToString(),
                });
        }

        public override object ToObject()
        {
            // create TextBox from CreateEditTextBoxViewModel
            var validation = new List<BaseElementValidator>();
            if (IsRequired)
            {
                validation.Add(new BaseElementValidator(ElementValidationType.Required)
                    {
                        IsChecked = true
                    });
            }

            validation.AddRange(ElementValidators.Where(x => x.IsChecked).ToList());
            var obj = new MnTextBox { Title = Title, HelpText = HelpText };
            obj.Validations.AddRange(validation);
            return obj;
        }
    }
}