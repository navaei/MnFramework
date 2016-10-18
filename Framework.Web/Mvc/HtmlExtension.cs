using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;
using System.Web.WebPages.Html;
using Mn.Framework.Common.Forms;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Globalization;

namespace Mn.Framework.Web.Mvc
{
    public static class HtmlExtension
    {
        public static MvcHtmlString GenerateHiddenFields<TModel>(this HtmlHelper<TModel> Html, IMnBaseElement model)
        {
            var result = string.Empty;
            result += Html.Hidden("Name", model.Name).ToHtmlString();
            result += Html.Hidden("Title", model.Title).ToHtmlString();
            result += Html.Hidden("HelpText", model.HelpText).ToHtmlString();
            result += Html.Hidden("ElementId", model.ElementId).ToHtmlString();
            result += Html.Hidden("CurrentMode", model.CurrentMode).ToHtmlString();
            result += Html.Hidden("VisibleMode", model.VisibleMode).ToHtmlString();
            result += Html.Hidden("ModelType", model.GetType()).ToHtmlString();

            result = model.AccessRole.Aggregate(result, (current, accRole) => current + Html.HiddenFor(modelItem => accRole).ToHtmlString());

            return MvcHtmlString.Create(result);
        }

        public static Dictionary<string, object> AddAttr(this Dictionary<string, object> attrArray, string key, object value)//this HtmlHelper<TModel> Html,
        {
            var temp = attrArray.ToArray().ToList();
            temp.Add(new KeyValuePair<string, object>(key, value));
            return temp.ToDictionary(d => d.Key, d => d.Value);
        }
        public static MvcHtmlString Ang<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            //return new MvcHtmlString(typeof(TModel).Name.Replace("ViewModel", string.Empty) + "." + html.IdFor(expression));
            return new MvcHtmlString("Model." + html.IdFor(expression));
        }

        public static void SetValue(this System.Web.Mvc.ModelStateDictionary state, string key, object value)
        {
            state.SetModelValue(key, new ValueProviderResult(value, value.ToString(), CultureInfo.CurrentCulture));
        }

    }
}
