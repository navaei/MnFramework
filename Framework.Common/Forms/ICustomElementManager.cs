using System.Collections.Generic;

namespace Mn.Framework.Common.Forms
{
    public interface ICustomElementManager
    {
        List<ICustomElement> GetAllCustomeElement();
        void AddCustomeElement(ICustomElement customeElement);
    }
}