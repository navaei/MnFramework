using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms;

namespace Mn.Framework.Web.Model
{   
    public class MnFormViewModel : BaseViewModel
    {
        public static MnFormViewModel FromJbForm(MnForm jbform)
        {
            return Helper.AutoMapper.Map<MnForm, MnFormViewModel>(jbform);
        }

        public MnForm ToJbForm()
        {
            return Mn.Framework.Helper.AutoMapper.Map<MnFormViewModel, MnForm>(this);
        }

        public List<MnBaseElement> Elements { get; set; }

        public string Name => $"MnForm{Id}";

        public int Id { get; set; }

        public string Title { get; set; }

        public string HelpText { get; set; }

        public bool ReadOnly { get; set; }
    }
}
