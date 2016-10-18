using System.Collections.Specialized;
using System.Data.Entity;
using System.Web.Configuration;
using Mn.Framework.Common.Forms;

namespace Mn.Framework.Common.Model
{
    public class BaseDataContext : DbContext
    {
        public bool IsDisposed { get; private set; }
        public BaseDataContext(string connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }
        public BaseDataContext()
            : this((WebConfigurationManager.GetSection("jumbula") as NameValueCollection)["AzureDbConnection"]) // note: cannot use the helper here, see Helpers.cs
        {

        }

        //public DbSet<MnForm> JbForms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MnForm>().ToTable("JbForms");
            base.OnModelCreating(modelBuilder);
        }


        //public override int SaveChanges()
        //{
        //    var modifiedEntries = this.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        //    foreach (var dbEntityEntry in modifiedEntries)
        //    {
        //        if (dbEntityEntry.Entity is JbBaseEntity)
        //        {
        //            var jsonData = JsonConvert.SerializeObject(dbEntityEntry.Entity);
        //            dbEntityEntry.Property("JsonData").CurrentValue = jsonData;
        //        }
        //    }
        //    return base.SaveChanges();
        //}

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
