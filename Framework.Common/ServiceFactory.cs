using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Model;
using Microsoft.Practices.Unity;

namespace Mn.Framework.Common
{
    public class ServiceFactory : MarshalByRefObject
    {
        static IUnityContainer uContainer = new UnityContainer();
        private static Type dataContextType { get; set; }

        public static BaseDataContext DataContext
        {
            get
            {
                return Get<BaseDataContext>();
            }
        }
        public static void Initialise(IUnityContainer unityContainer)
        {
            uContainer = unityContainer;
        }

        public static T Get<T>()
        {
            return uContainer.Resolve<T>();
        }       
        public static object Get(Type type)
        {
            return uContainer.Resolve(type);
        }

    }
}
