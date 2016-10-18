using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Mn.Framework.Common;
using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Reflaction;
using System.Reflection;

namespace Mn.Framework.Business
{
    public class BaseBusiness<TEntity, TPrimaryKey, TDataContext> : IDisposable, IBaseBusiness<TEntity, TPrimaryKey>
        where TEntity : class, IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
        where TDataContext : DbContext
    {

        TDataContext _dbContext;
        public BaseBusiness()
        {

        }
        public BaseBusiness(TDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual BaseDataContext DataContext
        {
            get
            {
                if (_dbContext != null)
                    return _dbContext as BaseDataContext;
                return ServiceFactory.DataContext;
            }
        }
        public virtual bool AllowSerialization
        {
            get
            {
                return DataContext.Configuration.ProxyCreationEnabled;
            }
            set
            {
                DataContext.Configuration.ProxyCreationEnabled = !value;
            }
        }
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return DataContext.Set<TEntity>().Where(predicate).SingleOrDefault();
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }
        public virtual TEntity Get(TPrimaryKey Id)
        {
            return GetList().SingleOrDefault(x => x.Id.Equals(Id));
        }

        //public virtual T Get<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        //{
        //    if (predicate != null)
        //    {
        //        using (DataContext)
        //        {
        //            return DataContext.Set<T>().Where(predicate).SingleOrDefault();
        //        }
        //    }
        //    else
        //    {
        //        throw new ApplicationException("Predicate value must be passed to Get<T>.");
        //    }
        //}

        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return DataContext.Set<TEntity>().Where(predicate);
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }

        public virtual IQueryable<TEntity> GetList<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy)
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }

        public virtual IQueryable<TEntity> GetList<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            try
            {
                return GetList().OrderBy(orderBy);
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }

        public virtual IQueryable<TEntity> GetList()
        {
            return GetList<TEntity>();
        }
        public virtual IQueryable<T> GetList<T>() where T : class, IBaseEntity<TPrimaryKey>
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }
        public virtual IQueryable<T> GetList2<T>() where T : BaseEntity
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }

        public virtual IQueryable<TEntity> Include(string relatedEntity)
        {
            try
            {
                return DataContext.Set<TEntity>().Include(relatedEntity);
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }

        public virtual IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] selectors)
        {
            try
            {
                var contx = DataContext.Set<TEntity>();
                foreach (var selector in selectors)
                    contx.Include(selector);
                return contx;
            }
            catch (Exception e)
            {
                throw new MnDbException(e);
            }
        }
        //public virtual IQueryable<TEntity> Include<TEntity>(this ObjectQuery<TEntity> query, Expression<Func<TEntity, object>> selector)
        //{
        //    string propertyName = GetPropertyName(selector);
        //    return query.Include(propertyName);
        //}
        public virtual OperationStatus Save()
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error saving " + typeof(TEntity) + ".", exp);
            }

            return opStatus;
        }

        public virtual OperationStatus Create(TEntity entity)
        {
            return Create<TEntity>(entity);
        }
        public virtual OperationStatus Create<T>(T entity) where T : class, IBaseEntity<TPrimaryKey>
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                // var metaData = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                //.SingleOrDefault(p => p.PropertyType == typeof(MetaData));
                // if (metaData != null)
                //     metaData.SetValue(entity, new MetaData() { DateCreated = DateTime.UtcNow, DateUpdated = DateTime.UtcNow });
                //entity.MetaData = new MetaData() { DateCreated = DateTime.UtcNow, DateUpdated = DateTime.UtcNow };
                DataContext.Set<T>().Add(entity);
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }


            return opStatus;
        }

        public virtual OperationStatus Delete(TEntity entity)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                DataContext.Set<TEntity>().Remove(entity);
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }

        public virtual OperationStatus Delete(TPrimaryKey entityId)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                var entity = DataContext.Set<TEntity>().Find(entityId);
                DataContext.Set<TEntity>().Remove(entity);
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }
        public virtual OperationStatus Delete<T>(TPrimaryKey entityId) where T : class, IBaseEntity<TPrimaryKey>
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                var entity = DataContext.Set<T>().Find(entityId);
                DataContext.Set<T>().Remove(entity);
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
                opStatus.Exception = exp;
            }

            return opStatus;
        }
        public virtual OperationStatus DeleteWhere(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                IQueryable<TEntity> query = DataContext.Set<TEntity>();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                foreach (var entity in query)
                {
                    DataContext.Entry(entity).State = EntityState.Deleted;
                }
                var count = DataContext.SaveChanges();
                opStatus.Status = count > 0;
                opStatus.RecordsAffected = count;
                return opStatus;
            }
            catch (Exception exp)
            {
                return OperationStatus.CreateFromException("Error creating " + typeof(TEntity) + ".", exp);
            }
        }
        public virtual OperationStatus Update(TEntity entity)
        {
            return Update<TEntity>(entity);
        }
        public virtual OperationStatus Update<T>(T entity) where T : class, IBaseEntity<TPrimaryKey>
        {

            OperationStatus opStatus = new OperationStatus { Status = true };
            if (entity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = DataContext.Entry<T>(entity);
            //entity.MetaData.DateUpdated = DateTime.UtcNow;

            var set = DataContext.Set<TEntity>();
            TEntity attachedEntity = set.Local.SingleOrDefault(e => e.Id.Equals(entity.Id));  // You need to have access to key

            if (attachedEntity != null)
            {
                var attachedEntry = DataContext.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
            }
            else
            {
                entry.State = EntityState.Modified; // This should attach entity
            }
            try
            {
                opStatus.Status = DataContext.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                opStatus = OperationStatus.CreateFromException("Error updating " + typeof(TEntity) + ".", ex);
                opStatus.Exception = ex;
            }
            // }

            return opStatus;
        }

        public virtual OperationStatus UpdateProperty(TPrimaryKey entityId, string propertyName, object propertyValue)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                var entity = DataContext.Set<TEntity>().Find(entityId);
                DataContext.Entry(entity).Property(propertyName).CurrentValue = propertyValue;
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus.Exception = exp;
                opStatus = OperationStatus.CreateFromException("Error updating " + typeof(TEntity) + ".", exp);
            }

            return opStatus;
        }

        public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                //opStatus.RecordsAffected = DataContext.ExecuteStoreCommand(cmdText, parameters);
                //TODO: [Papa] = Have not tested this yet.
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText, parameters);
            }
            catch (Exception exp)
            {
                OperationStatus.CreateFromException("Error executing store command: ", exp);
            }
            return opStatus;
        }

        public void Dispose()
        {
            //if (DataContext != null)
            //    DataContext.Dispose();
            //if (DataContext != null)
            //{
            //    DataContext.Dispose();
            //    //_dataContext = null;
            //}
        }

        #region privateMethod
        //private string GetPropertyName<T>(Expression<Func<T, object>> expression)
        //{
        //    MemberExpression memberExpr = expression.Body as MemberExpression;
        //    if (memberExpr == null)
        //        throw new ArgumentException("Expression body must be a member expression");
        //    return memberExpr.Member.Name;
        //}
        #endregion
    }

    public class BaseBusiness<TEntity> : BaseBusiness<TEntity, Int64>
          where TEntity : class, IBaseEntity<Int64>
    {

    }

    public class BaseBusiness<TEntity, TPrimaryKey> : BaseBusiness<TEntity, TPrimaryKey, BaseDataContext>
        where TEntity : class, IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {

    }


}
