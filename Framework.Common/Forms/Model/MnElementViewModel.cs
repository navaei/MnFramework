using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    public class MnBaseElementViewModel
    {
        public MnBaseElementViewModel()
        {
            Validations = new List<MnElementItem>();
            ElementValidators = new List<BaseElementValidator>();
        }

        [Required]
        public string Title { get; set; }

        public string HelpText { get; set; }

        public IEnumerable<MnElementItem> Validations { get; set; }

        public int SelectedValidationId { get; set; }

        public IList<BaseElementValidator> ElementValidators { get; set; }        

        public int Order { get; set; }

        public virtual object ToObject()
        {
            throw new NotImplementedException("ToObject not implemented");
        }
    }
}
