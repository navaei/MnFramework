using System.Collections.Generic;
using System.Linq;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    public class CreateEditDatePickerViewModel : MnBaseElementViewModel
    {
        public bool IsRequired { get; set; }

        public CreateEditDatePickerViewModel()
        {
            // load supported validation
            SeedSupportedValidation();
        }

        public CreateEditDatePickerViewModel(MnDatePicker datePicker)
        {
            Title = datePicker.Title;
            HelpText = datePicker.HelpText;
            IsRequired = datePicker.Validations.Any(x => x.Type == ElementValidationType.Required);
            SelectedValidationId = (int)datePicker.Validations.First(x => x.Type != ElementValidationType.Required).Type;

            // file ElementValidators by default validation and default value
            SeedSupportedValidation();

            // if my datepicker has validation replace it validation by default ElementValidators
            foreach (var item in datePicker.Validations.Where(x => x.Type != ElementValidationType.Required))
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
                    new ElementDatePickerRangeValidation()
                };

            ElementValidators = validationList;
            Validations = validationList.Select(p => new MnElementItem()
                {
                    Value = ((int)(p.Type)).ToString(),
                    Text = p.Type.ToString(),
                });
        }

        public override object ToObject()
        {
            // create datepicker from CreateEditDatePickerViewModel
            var validation = new List<BaseElementValidator>();
            if (IsRequired)
            {
                validation.Add(new BaseElementValidator(ElementValidationType.Required)
                    {
                        IsChecked = true
                    });
            }

            validation.AddRange(ElementValidators.Where(x => x.IsChecked).ToList());

            var obj = new MnDatePicker() { Title = Title, HelpText = HelpText };
            obj.Validations.AddRange(validation);
            return obj;
        }
    }
}