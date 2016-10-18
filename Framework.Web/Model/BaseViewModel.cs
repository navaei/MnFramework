using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Mn.Framework.Common.Model;

namespace Mn.Framework.Web.Model
{
    public abstract class BaseViewModel : IBaseViewModel<long>
    {
        public long Id { get; set; }

        public override string ToString()
        {
            return base.ToString().Split('.')[base.ToString().Split('.').Length - 1].Replace("ViewModel", "");
        }
    }

    public abstract class BaseViewModel<TBaseEntity, TPrimaryKey> : IBaseViewModel<TPrimaryKey>
        where TBaseEntity : BaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        public TPrimaryKey Id { get; set; }
        public override string ToString()
        {
            return base.ToString().Split('.')[base.ToString().Split('.').Length - 1].Replace("ViewModel", "");
        }
        public TModel ToModel<TModel>() where TModel : class ,new()
        {
            return Mn.Framework.Helper.AutoMapper.Map<TModel>(this);
        }
        public TModel ToModel<TModel>(TModel dbEntity) where TModel : BaseEntity<TPrimaryKey>, new()
        {
            Mn.Framework.Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TModel;
        }
    }

    public abstract class BaseViewModel<TBaseEntity> : IBaseViewModel<long>
        where TBaseEntity : BaseEntity
    {
        public long Id { get; set; }
        public string PageTitle { get; set; }
        public override string ToString()
        {
            return base.ToString().Split('.')[base.ToString().Split('.').Length - 1].Replace("ViewModel", "");
        }
        public TModel ToModel<TModel>() where TModel : class ,new()
        {
            return Mn.Framework.Helper.AutoMapper.Map<TModel>(this);
        }
        public TModel ToModel<TModel>(TModel dbEntity) where TModel : BaseEntity, new()
        {
            Mn.Framework.Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TModel;
        }
    }
}
