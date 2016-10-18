using System;
using System.Web.Mvc;
using Mn.Framework.Common.Forms.Validation;

namespace Mn.Framework.Web.Mvc
{
    public class MnElementValidatorModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {

            var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".QVModelType");
            Type type;
            if (typeValue == null)
            {
                type = typeof(BaseElementValidator);
                var model = Activator.CreateInstance(type);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
                return model;
            }
            else
            {
                type = Type.GetType(
                    (string)typeValue.ConvertTo(typeof(string)) + ",Mn.Framework.Common", true);
                var model = Activator.CreateInstance(type);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
                return model;
            }

        }
    }
}
