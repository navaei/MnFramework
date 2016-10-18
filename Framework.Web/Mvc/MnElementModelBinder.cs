using System;
using System.Web.Mvc;

namespace Mn.Framework.Web.Mvc
{
    public class MnElementModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            string typename;
            Type type;
            var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".ModelType");
            if (typeValue == null)
                typename = "Mn.Framework.Common.Forms.Jb" + bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".TypeTitle").ConvertTo(typeof(string));
            else
                typename = (string)typeValue.ConvertTo(typeof(string));
            
            if (typename.Contains("Mn.Framework.Common"))
                type = Type.GetType(typename + ",Mn.Framework.Common", true);
            else
                type = Type.GetType(typename + "," + typename.Split('.')[0], true);
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }


    }

    public class CustomElementModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".ModelType");
            var type = Type.GetType(
                (string)typeValue.ConvertTo(typeof(string)),
                true
            );
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }
    }
}