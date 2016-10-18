using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Common.Forms.Binding
{
    public class BindingConfig
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public List<BindingMapElement> MapElements { get; set; }
    }
}
