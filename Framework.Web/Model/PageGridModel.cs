using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Web.Model
{
    public class PageGridModel
    {
        public PageGridModel()
        {
            GridEvents = "onGridDataBound";
        }
        public string GridEvents { get; set; }
        public ColumnActionMenu GridMenu { get; set; }
    }
}
