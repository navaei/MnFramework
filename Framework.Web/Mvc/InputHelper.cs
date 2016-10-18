using System.Collections.Generic;
using Mn.Framework.Common.Forms;
using System.Web.WebPages.Html;

namespace Mn.Framework.Web.Mvc
{
    public class InputHelper
    {

        public static void InputAccessible(MnBaseElement model, string role, ref Dictionary<string, object> attributeList)
        {
            var mode = model.GetAccessMode(role);
            if (mode == AccessMode.Hidden)
            {
                attributeList.Add("hidden", "");
            }
            else if (mode == AccessMode.ReadOnly)
            {
                attributeList.Add("disabled", "");
            }
            else if (mode == AccessMode.Design)
            {
                attributeList.Add("data-design", "yes");
            }
        }

        public static void InputAccessible(MnBaseElement model, VisibilityMode visibleMode, ref Dictionary<string, object> attributeList)
        {
            if (model.VisibleMode != VisibilityMode.Both && model.VisibleMode != visibleMode)
            {
                attributeList.Add("hidden", "");
            }
        }      
    }
}
