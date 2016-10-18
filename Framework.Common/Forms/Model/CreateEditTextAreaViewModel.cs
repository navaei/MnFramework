using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    public class CreateEditTextAreaViewModel : MnBaseElementViewModel
    {
        public bool IsRequired { get; set; }

        public CreateEditTextAreaViewModel()
        {
            // load supported validation
            SeedSupportedValidation();
        }

        public CreateEditTextAreaViewModel(MnTextArea textArea)
        {
            Title = textArea.Title;
            HelpText = textArea.HelpText;
            IsRequired = textArea.Validations.Any(x => x.Type == ElementValidationType.Required);
            SelectedValidationId = (int)textArea.Validations.First(x => x.Type != ElementValidationType.Required).Type;

            // file ElementValidators by default validation and default value
            SeedSupportedValidation();

            // if my textBox has validation replace it validation by default ElementValidators
            foreach (var item in textArea.Validations.Where(x => x.Type != ElementValidationType.Required))
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
                    new ElementMaxLengthValidation()
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
            // create TextArea from CreateEditTextAreaViewModel
            var validation = new List<BaseElementValidator>();
            if (IsRequired)
            {
                validation.Add(new BaseElementValidator(ElementValidationType.Required)
                    {
                        IsChecked = true
                    });
            }

            validation.AddRange(ElementValidators.Where(x => x.IsChecked).ToList());

            var obj = new MnTextArea() { Title = Title, HelpText = HelpText };
            obj.Validations.AddRange(validation);
            return obj;
        }
    }
}