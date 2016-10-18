using System.Collections.Generic;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Common.Forms
{
    public interface IMnBaseElement
    {
        string ElementId { get; set; }
        string Name { get; set; }
        string Title { get; set; }
        string TypeTitle { get; set; }
        string HelpText { get; set; }
        ElementSize Size { get; set; }
        AccessMode CurrentMode { get; set; }
        VisibilityMode VisibleMode { get; set; }
        Dictionary<string, AccessMode> AccessRole { get; set; }
        List<BaseElementValidator> Validations { get; set; }
        object GetValue();
        void SetValue(object value);       
        bool Equals(IMnBaseElement mnElement);
    }
}