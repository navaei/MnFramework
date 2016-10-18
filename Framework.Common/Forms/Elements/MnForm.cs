using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Mn.Framework.Common.Forms.Binding;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Common.Model;
using Mn.Framework.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnFormConverter))]
    public class MnForm : BaseEntity
    {

        private List<IMnBaseElement> _elements;
        private AccessMode _currentMode;
        private VisibilityMode _visibleMode;
        private string _stringElements { get; set; }

        public MnForm()
        {
            _elements = new List<IMnBaseElement>();
            AccessRole = new Dictionary<string, AccessMode>();
            //CreatedDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
        }
        public MnForm(VisibilityMode visibleMode)
            : this()
        {
            this.VisibleMode = visibleMode;
        }

        [NotMapped]
        public VisibilityMode VisibleMode
        {
            get { return _visibleMode; }
            set
            {
                _visibleMode = value;
                //SetElementsVisibleMode(value);
            }
        }

        [NotMapped]
        public AccessMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                //SetElementsCurrentMode();
            }
        }

        public string RefEntityName { get; set; }
        public int RefEntityId { get; set; }


        [NotMapped]
        [JsonProperty(IsReference = true)]
        public List<IMnBaseElement> Elements
        {
            get
            {

                if (!_elements.Any() && !string.IsNullOrEmpty(_stringElements))
                {
                    var jbc = new MnElementConverter();
                    var jsonObject = JArray.Parse(_stringElements);
                    var props = jsonObject.ToObject<List<object>>();
                    _elements = props.Select(prop => jbc.GetJbElement(prop as JObject)).ToList();
                }
                return _elements;
            }
            set { _elements = value; }
        }

        [JsonIgnore]
        public string JsonElements
        {
            get
            {
                if (string.IsNullOrEmpty(_stringElements) && Elements != null && Elements.Any())
                    _stringElements = JsonHelper.JsonSerializer(Elements);
                return _stringElements;
            }
            set
            {
                _stringElements = value;
            }
        }

        public string Title
        {
            get;
            set;
        }

        #region metaData

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime CreatedDate { get; set; }

        public virtual DateTime LastModifiedDate { get; set; }

        #endregion

        public string HelpText { get; set; }

        [NotMapped]
        public Dictionary<string, AccessMode> AccessRole
        {
            get;
            set;
        }

        public void SetElementsCurrentMode(AccessMode currentMode)
        {
            Elements.ForEach(e =>
             {
                 e.CurrentMode = currentMode;
                 if (e is MnSection)
                 {
                     (e as MnSection).Elements.ForEach(e2 =>
                     {
                         e2.CurrentMode = currentMode;
                         if (e2 is MnSection)
                         {
                             (e2 as MnSection).Elements.ForEach(e3 => { e3.CurrentMode = currentMode; });
                         }
                     });
                 }
             });
        }
        public void ChangeElementsMode(AccessMode fromMode, AccessMode toMode)
        {
            Elements.ForEach(e =>
            {
                if (e.CurrentMode == fromMode)
                    e.CurrentMode = toMode;
                if (e is MnSection)
                {
                    (e as MnSection).Elements.ForEach(e2 =>
                    {
                        if (e2.CurrentMode == fromMode)
                            e2.CurrentMode = toMode;
                        if (e2 is MnSection)
                        {
                            (e2 as MnSection).Elements.Where(e2e => e2e.CurrentMode == fromMode).ToList()
                                .ForEach(e3 =>
                                    {
                                        e3.CurrentMode = toMode;
                                    });
                        }
                    });
                }
            });
        }
        public void SetElementsVisibleMode(VisibilityMode visibleMode)
        {
            Elements.ForEach(e =>
            {
                e.VisibleMode = visibleMode;
                if (e is MnSection)
                {
                    (e as MnSection).Elements.ForEach(e2 =>
                    {
                        e2.VisibleMode = visibleMode;
                        if (e2 is MnSection)
                        {
                            (e2 as MnSection).Elements.ForEach(e3 => { e3.VisibleMode = visibleMode; });
                        }
                    });
                }
            });
        }

        public IMnBaseElement GetElement(string elementName)
        {
            var element = this.Elements.SingleOrDefault(e => e.Name == elementName);
            if (element == null)
            {
                var sections = this.Elements.Where(e => e is MnSection);
                foreach (var section in sections)
                {
                    var elements = (section as MnSection).Elements;
                    foreach (var element2 in elements)
                    {
                        if (element2.Name == elementName)
                        {
                            element = element2;
                            break;
                        }
                    }
                }
            }
            return element;
        }

        public TElement GetElement<TElement>(string elementName) where TElement : MnBaseElement
        {
            var element = this.Elements.SingleOrDefault(e => e is TElement && e.Name == elementName);
            if (element == null)
            {
                var sections = this.Elements.Where(e => e is MnSection);
                foreach (var section in sections)
                {
                    var elements = (section as MnSection).Elements;
                    foreach (var element2 in elements)
                    {
                        if (element2 is TElement && element2.Name == elementName)
                        {
                            element = element2;
                            break;
                        }
                    }
                }
            }

            return element as TElement;
        }

        public TElement GetElementBySectionName<TElement>(string elementName, string sectionName) where TElement : MnBaseElement
        {
            var section = GetElement<MnSection>(sectionName);
            var element = section.Elements.SingleOrDefault(e => e is TElement && e.Name == elementName);
            if (element == null)
                return null;
            return element as TElement;
        }
    }
}
