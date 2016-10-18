using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Mn.Framework.Common.Forms
{
    public static class FormExtension
    {

        public static void RightCopy(this MnForm sourceForm, MnForm destForm, bool replaceElements = false)
        {
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = destForm.Elements.SingleOrDefault(e => e != null && e.ElementId == rootElement.ElementId);
                    if (section != null)
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            var element = destForm.Elements.SingleOrDefault(e => e != null && e.ElementId == selemnt.ElementId);
                            if (element != null)
                            {
                                try
                                {
                                    if (replaceElements)
                                    {
                                        (section as MnSection).Elements.Remove(element);
                                        (section as MnSection).Elements.Add(selemnt);
                                    }
                                    else
                                    {
                                        element = selemnt;
                                    }
                                }
                                catch
                                {

                                }

                            }
                        }
                }
            }
        }

        //public static bool TryGet<T>(this IEnumerable<MnBaseElement> sourceElements, MnBaseElement searchElement, out T newElement) where T : MnBaseElement, new()
        //{
        //    newElement = new T();
        //    //Func<MnBaseElement, bool> filter = e => e.Equals(searchElement);
        //    if (sourceElements.Any(e => e.Equals(searchElement)))
        //    {
        //        var tempElement = sourceElements.FirstOrDefault(e => e.Title.Equals(searchElement.Title));
        //        if (tempElement == null)
        //            tempElement = sourceElements.FirstOrDefault(e => e.Name.Equals(searchElement.Name));
        //        if (tempElement == null)
        //            tempElement = sourceElements.FirstOrDefault(e => e.ElementId.Equals(searchElement.ElementId));
        //        if (tempElement != null)
        //        {
        //            newElement = tempElement as T;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public static bool TryGet<T>(this IEnumerable<IMnBaseElement> sourceElements, IMnBaseElement searchElement, out T newElement) where T : MnBaseElement, new()
        {
            newElement = new T();
            //Func<MnBaseElement, bool> filter = e => e.Equals(searchElement);
            if (sourceElements.Any(e => e.Equals(searchElement)))
            {
                var tempElement = sourceElements.FirstOrDefault(e => e.Title.EqualsTrim(searchElement.Title));
                if (tempElement == null)
                    tempElement = sourceElements.FirstOrDefault(e => e.Name.Equals(searchElement.Name));
                if (tempElement == null)
                    tempElement = sourceElements.FirstOrDefault(e => e.ElementId.Equals(searchElement.ElementId));
                if (tempElement != null)
                {
                    newElement = tempElement as T;
                    return true;
                }
            }
            return false;
        }
        public static MnForm RightCopyNewByAny(this MnForm sourceForm, MnForm destForm, bool replaceElements = false)
        {
            var newForm = new MnForm();
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    MnSection section;
                    if (destForm.Elements.TryGet(rootElement, out section))
                    {
                        if (!newForm.Elements.Any(s => s.ElementId == section.ElementId))
                            newForm.Elements.Add(new MnSection()
                            {
                                ElementId = section.ElementId,
                                Name = section.Name,
                                Title = section.Title
                            });
                        foreach (var sourceElement in (rootElement as MnSection).Elements)
                        {
                            MnBaseElement element;
                            if ((section as MnSection).Elements.TryGet(sourceElement, out element))
                            {
                                if (replaceElements)
                                    (newForm.Elements.Single(e => e.ElementId == section.ElementId) as MnSection).Elements.Add(sourceElement);
                                else
                                    (newForm.Elements.Single(e => e.ElementId == section.ElementId) as MnSection).Elements.Add(element);
                            }
                        }
                    }
                }
            }
            return newForm;
        }
        public static void RightReplace(this MnForm sourceForm, MnForm destForm)
        {
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = destForm.Elements.SingleOrDefault(e => e != null && e.ElementId == rootElement.ElementId);
                    if (section != null)
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            var element = destForm.Elements.SingleOrDefault(e => e != null && e.ElementId == selemnt.ElementId);
                            if (element != null)
                            {
                                try
                                {
                                    (section as MnSection).Elements.Remove(element);
                                    (section as MnSection).Elements.Add(selemnt);
                                }
                                catch
                                {

                                }
                            }
                        }
                }
            }
        }
        public static void CopyByTitle(this MnForm sourceForm, MnForm destForm)
        {
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = destForm.Elements.SingleOrDefault(e => e != null &&
                        ((string.IsNullOrEmpty(e.Title) &&
                        (e.ElementId == rootElement.ElementId || e.Name == rootElement.Name)) ||
                        e.Title.EqualsTrim(rootElement.Title)));

                    if (section != null)
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            var element = (section as MnSection).Elements.SingleOrDefault(e => e != null &&
                                ((string.IsNullOrEmpty(e.Title) && (e.ElementId == selemnt.ElementId || e.Name == selemnt.Name)) ||
                                e.Title.EqualsTrim(selemnt.Title)));
                            if (element == null)
                                (section as MnSection).Elements.Add(selemnt);
                            else
                                element = selemnt;
                        }
                    else
                        destForm.Elements.Add(rootElement);
                }
            }
        }
        public static void CopyAny(this MnForm sourceForm, MnForm destForm)
        {
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = destForm.Elements.SingleOrDefault(e => e != null && e.Equals(rootElement));

                    if (section != null)
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            var element = (section as MnSection).Elements.SingleOrDefault(e => e != null && e.Equals(rootElement));
                            if (element == null)
                                (section as MnSection).Elements.Add(selemnt);
                            else
                                element = selemnt;
                        }
                    else
                        destForm.Elements.Add(rootElement);
                }
            }
        }
        public static void BindingByTitle(this MnForm destForm, MnForm sourceForm)
        {
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = destForm.Elements.SingleOrDefault(e => e != null &&
                        ((string.IsNullOrEmpty(e.Title) &&
                        (e.ElementId == rootElement.ElementId || (e.Name != null && e.Name == rootElement.Name))) ||
                        e.Title.EqualsTrim(rootElement.Title)));

                    if (section != null)
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            var element = (section as MnSection).Elements.SingleOrDefault(e => e != null &&
                                ((string.IsNullOrEmpty(e.Title) && (e.ElementId == selemnt.ElementId || (e.Name != null && e.Name == selemnt.Name))) ||
                                e.Title.EqualsTrim(selemnt.Title)));
                            if (element != null)
                            {
                                if (element.GetType() != selemnt.GetType())
                                    continue;
                                if (element is MnDropDown)
                                    if (!(element as MnDropDown).EqualsItems(selemnt as MnDropDown))
                                        continue;

                                element.SetValue(selemnt.GetValue());
                            }

                        }
                }
            }
        }
        public static void BindingAny(this MnForm destForm, MnForm sourceForm)
        {
            foreach (var rootElement in sourceForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    if (!destForm.Elements.Any(e => e != null &&
                        (e.ElementId == rootElement.ElementId || (e.Name != null && e.Name == rootElement.Name) ||
                        e.Title.EqualsTrim(rootElement.Title))))
                        continue;

                    var section = destForm.Elements.FirstOrDefault(e => e != null &&
                        (e.ElementId == rootElement.ElementId || (e.Name != null && e.Name == rootElement.Name) ||
                        e.Title.EqualsTrim(rootElement.Title)));

                    if (section != null)
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            if (!(section as MnSection).Elements.Any(e => e != null &&
                                (e.ElementId == selemnt.ElementId || e.Title.EqualsTrim(selemnt.Title) || (e.Name != null && e.Name == selemnt.Name))))
                                continue;

                            var element = (section as MnSection).Elements.FirstOrDefault(e => e != null &&
                                (e.ElementId == selemnt.ElementId || e.Title.EqualsTrim(selemnt.Title) || (e.Name != null && e.Name == selemnt.Name)));
                            if (element != null)
                            {
                                if (element.GetType() != selemnt.GetType())
                                    continue;
                                if (element is MnDropDown)
                                    if (!(element as MnDropDown).EqualsItems(selemnt as MnDropDown))
                                        continue;
                                if (element is MnAddress)
                                {
                                    (element as MnAddress).AddressLine1 = (selemnt as MnAddress).AddressLine1;
                                    (element as MnAddress).AddressLine2 = (selemnt as MnAddress).AddressLine2;
                                    (element as MnAddress).City = (selemnt as MnAddress).City;
                                    (element as MnAddress).State = (selemnt as MnAddress).State;
                                    (element as MnAddress).Zip = (selemnt as MnAddress).Zip;
                                }

                                element.SetValue(selemnt.GetValue());
                            }

                        }
                }
            }
        }

        //public static void LeftCopy(this MnForm sourceForm, MnForm destForm)
        //{
        //    foreach (var rootElement in sourceForm.Elements)
        //    {
        //        if (rootElement is MnSection)
        //        {
        //            var section = destForm.Elements.SingleOrDefault(e => e != null && e.ElementId == rootElement.ElementId);
        //            if (section != null)
        //                destForm.Elements.Add(section);
        //            foreach (var selemnt in (rootElement as MnSection).Elements)
        //            {
        //                var element = (section as MnSection).Elements.SingleOrDefault(e => e != null && e.ElementId == selemnt.ElementId);
        //                if (element != null)
        //                {
        //                    (section as MnSection).Elements.Add(element);
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        element = selemnt;
        //                    }
        //                    catch
        //                    {

        //                    }

        //                }
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //}
        //public static void LeftCopyByName(this MnForm sourceForm, MnForm destForm)
        //{
        //    foreach (var rootElement in sourceForm.Elements)
        //    {
        //        if (rootElement is MnSection)
        //        {
        //            var section = destForm.Elements.SingleOrDefault(e => e != null && e.Name == rootElement.Name);
        //            if (section != null)
        //                foreach (var selemnt in (rootElement as MnSection).Elements)
        //                {
        //                    var element = (section as MnSection).Elements.SingleOrDefault(e => e != null && e.Name == selemnt.Name);
        //                    if (element == null)
        //                        (section as MnSection).Elements.Add(selemnt);
        //                    else
        //                        element = selemnt;
        //                }
        //            else
        //                destForm.Elements.Add(rootElement);
        //        }
        //    }
        //}
        //public static void LeftCopyByAny(this MnForm sourceForm, MnForm destForm)
        //{
        //    foreach (var rootElement in sourceForm.Elements)
        //    {
        //        if (rootElement is MnSection)
        //        {
        //            var section = destForm.Elements.SingleOrDefault(e => e != null && (e.ElementId == rootElement.ElementId || e.Name == rootElement.Name || e.Title == rootElement.Title));
        //            if (section != null)
        //                foreach (var selemnt in (rootElement as MnSection).Elements)
        //                {
        //                    var element = (section as MnSection).Elements.SingleOrDefault(e => e != null && (e.ElementId == selemnt.ElementId || e.Name == selemnt.Name || e.Title == selemnt.Title));
        //                    if (element == null)
        //                        (section as MnSection).Elements.Add(selemnt);
        //                    else
        //                        element = selemnt;
        //                }
        //            else
        //                destForm.Elements.Add(rootElement);
        //        }
        //    }
        //}

        public static MnForm DifferenceByValue(this MnForm firstForm, MnForm secondForm)
        {
            var newForm = new MnForm();
            foreach (var rootElement in firstForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = secondForm.Elements.SingleOrDefault(e => e != null && (e.ElementId == rootElement.ElementId ||
                        e.Name == rootElement.Name ||
                        e.Title.EqualsTrim(rootElement.Title)));
                    if (section != null)
                    {
                        foreach (var selemnt in (rootElement as MnSection).Elements)
                        {
                            var element =
                                (section as MnSection).Elements.SingleOrDefault(
                                    e =>
                                    e != null &&
                                    (e.ElementId == selemnt.ElementId || (e.Name != null && e.Name == selemnt.Name) ||
                                     e.Title.EqualsTrim(selemnt.Title)));
                            if (element != null && element.GetValue() != selemnt.GetValue())
                            {
                                newForm.Elements.Add(element);
                            }

                        }
                    }
                }
            }
            return newForm;
        }

        public static IEnumerable<string> DifferenceByValue(this MnForm firstForm, MnForm secondForm, string message, string emptyNewValueMessage, string emptyOldValueMessage)
        {
            string result = string.Empty;
            foreach (var rootElement in firstForm.Elements)
            {
                if (rootElement is MnSection)
                {
                    var section = secondForm.Elements.SingleOrDefault(e => e != null
                        && ((string.IsNullOrEmpty(e.Title) && (e.ElementId == rootElement.ElementId || (e.Name != null && e.Name == rootElement.Name)))
                        || e.Title.EqualsTrim(rootElement.Title)));
                    if (section != null)
                    {
                        foreach (var oldElement in (rootElement as MnSection).Elements)
                        {
                            var newElement =
                                (section as MnSection).Elements.SingleOrDefault(
                                    e =>
                                    e != null &&
                                    ((string.IsNullOrEmpty(e.Title) && (e.ElementId == oldElement.ElementId || (e.Name != null && e.Name == oldElement.Name))) ||
                                     e.Title.EqualsTrim(oldElement.Title)));
                            if (newElement.GetValue() == null && oldElement.GetValue() == null)
                                continue;
                            if (newElement.GetValue() != null && oldElement.GetValue() == null)
                                yield return string.Format(emptyOldValueMessage, oldElement.Title, section.Title, newElement.GetValue());

                            if (newElement.GetValue() == null && oldElement.GetValue() != null)
                                yield return string.Format(emptyNewValueMessage, oldElement.Title, section.Title, oldElement.GetValue());

                            if (!newElement.GetValue().Equals(oldElement.GetValue()))
                                yield return string.Format(message, oldElement.Title, section.Title, oldElement.GetValue(), newElement.GetValue());
                        }
                    }
                }
            }
        }

        public static void DataBinding(this MnForm form, MnForm secondform)
        {
            form.BindingAny(secondform);
        }
    }
}
