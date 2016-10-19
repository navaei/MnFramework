using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Reflaction
{
    public class BaseActivator : MarshalByRefObject
    {
        public static T Create<T>()
        {
            return (T)Activator.CreateInstance<T>();
        }
        public static T Create<T>(string fullTypeName)
        {
            return (T)Activator.CreateInstance(Type.GetType(fullTypeName));
        }
        public static object Create(string fullTypeName)
        {
            return Activator.CreateInstance(Type.GetType(fullTypeName));
        }
        public static T Create<T>(Type entityType)
        {
            return (T)Activator.CreateInstance(entityType);
        }
        public static dynamic Create(Type entityType)
        {
            return Activator.CreateInstance(entityType);
        }
    }
}
