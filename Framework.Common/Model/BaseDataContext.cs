using System.Collections.Specialized;
using System.Data.Entity;
using System.Web.Configuration;
using Mn.Framework.Common.Forms;

namespace Mn.Framework.Common.Model
{
    public class BaseDataContext : DbContext
    {
        public bool IsDisposed { get; private set; }
        public BaseDataContext(string connectionString) : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }
        public BaseDataContext() : this("DefaultConnectionString")
        {

        }      

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
