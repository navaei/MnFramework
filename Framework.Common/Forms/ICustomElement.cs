using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public interface ICustomElement : IMnBaseElement
    {

    }
}
