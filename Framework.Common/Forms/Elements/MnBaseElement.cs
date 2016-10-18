using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Mn.Framework.Common.Forms.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mn.Framework.Common.Forms
{
    //[XmlInclude(typeof(MnTextBox))]
    //[XmlInclude(typeof(MnCheckBox))]
    //[XmlInclude(typeof(MnSection))]
    //[XmlInclude(typeof(MnDropDown))]
    //[Serializable]
    public enum VisibilityMode
    {
        Both,
        Online,
        Offline,
    }
    public class MnBaseElement : IMnBaseElement
    {
        public virtual string ElementId { get; set; }
        public string TypeTitle { get; set; }
        public virtual string Name { get; set; }
        public string Title { get; set; }
        public string HelpText { get; set; }
        public ElementSize Size { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VisibilityMode VisibleMode { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessMode CurrentMode { get; set; }
        public Dictionary<string, AccessMode> AccessRole { get; set; }
        public List<BaseElementValidator> Validations { get; set; }

        public MnBaseElement()
        {
            TypeTitle = this.GetType().Name.Replace("Jb", "");
            ElementId = this.GetType().Name + StaticRandom.Instance.Next(1, int.MaxValue);
            //Debug.Print("Element ID " + ElementId);
            AccessRole = new Dictionary<string, AccessMode>();
            Validations = new List<BaseElementValidator>();
        }

        public virtual void AddAccess(string roleName, AccessMode mode)
        {
            roleName = roleName.ToLower();
            if (AccessRole.Any(x => x.Key == roleName))
                AccessRole.Remove(roleName);

            AccessRole.Add(roleName, mode);
        }
        public virtual AccessMode GetAccessMode(string roleName)
        {
            roleName = roleName.ToLower();
            if (AccessRole.Any(x => x.Key == roleName))
                return AccessRole[roleName];
            return AccessMode.Edit;
        }

        public virtual object GetValue()
        {
            var property = this.GetType().GetProperty("Value");
            if (property != null)
            {
                return property.GetValue(this, null);
            }
            throw new Exception("Method not implemented");
        }
        public virtual void SetValue(object value)
        {
            var property = this.GetType().GetProperty("Value");
            if (property != null)
                property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
        }

        public bool Equals(IMnBaseElement mnElement)
        {
            if (this.ElementId == mnElement.ElementId ||
                (this.Name != null && this.Name.Equals(mnElement.Name, StringComparison.CurrentCultureIgnoreCase)) ||
              (this.Title.EqualsTrim(mnElement.Title)))
                return true;
            return false;
        }
    }
}