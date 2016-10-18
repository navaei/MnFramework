

namespace Mn.Framework.Common.Forms
{
    ///<summary>
    ///This is Base Elemnt that using in other elemnts
    /// do not have UI implemention
    ///</summary>
    public class MnElementItem
    {

        public MnElementItem()
        {

        }

        public MnElementItem(string value, string text)
        {
            Value = value;
            Text = text;
        }

        public string Value { get; set; }
        public string Text { get; set; }

        public static explicit operator MnElementItem(MnBaseElement mnElement)
        {
            return new MnElementItem(mnElement.Title, mnElement.GetValue().ToString());
        }
    }

    public class JbSelectedItem
    {        

        public JbSelectedItem(bool value, string title)
        {
            Value = value;
            Title = title;
        }

        public bool Value { get; set; }
        public string Title { get; set; }

    }
}