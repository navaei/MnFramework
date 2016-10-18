using System.ComponentModel;

namespace Mn.Framework.Common.Forms
{
    public enum ElementSize
    {
        [Description("input-mini")]
        Mini = -10,
        [Description("input-small")]
        Small = -5,
        [Description("input-medium")]
        Medium = 0,
        [Description("input-large")]
        Large = 5,
        [Description("input-xlarge")]
        XLarge = 10,
        [Description("input-xxlarge")]
        XXLarge = 15
    }
}
