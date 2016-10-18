using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Common.Model
{
    public abstract class MnException : Exception
    {
        public MnException(string s, Exception e)
            : base(s, e)
        {

        }
    }

    #region Database Exceptions
    public class MnDbException : MnException
    {
        public MnDbException(Exception e)
            : base("Jumbula database exception", e)
        {

        }
    }    
    #endregion    
}
