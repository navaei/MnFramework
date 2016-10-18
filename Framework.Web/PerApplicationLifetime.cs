using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mn.Framework.Web
{
    public class PerApplicationLifetime : LifetimeManager
    {
        // This is very important part and the reason why I believe mentioned
        // PerCallContext implementation is wrong.
        private readonly Guid _key = Guid.NewGuid();

        public override object GetValue()
        {
            return HttpContext.Current.Application[_key.ToString()];
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Application[_key.ToString()] = newValue;
        }

        public override void RemoveValue()
        {
            //var obj = GetValue();
            HttpContext.Current.Application.Remove(_key.ToString());
        }
    }
}
