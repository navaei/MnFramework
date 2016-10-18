using System.Collections.Generic;
using Mn.Framework.Common.Forms.JsonFormatter;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnSection : MnBaseElement
    {
        public MnSection()
        {
            Elements = new List<IMnBaseElement>();
        }

        public List<IMnBaseElement> Elements { get; set; }
    }
}