using Mn.Framework.Common.Forms.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Common.Forms
{
    public static class Extensions
    {
        public static void AddRange(this IList<BaseElementValidator> list, List<BaseElementValidator> newItems)
        {
            foreach (var item in newItems)
            {
                list.Add(item);
            }
        }
    }
}
