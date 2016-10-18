using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Mn.Framework.Common.Model
{
    public abstract class BaseEntity : IBaseEntity
    {

        [Key]
        public virtual Int64 Id { get; set; }
        public TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new()
        {
            var vm = new TViewModel();
            vm = Mn.Framework.Helper.AutoMapper.Map<TViewModel>(this);
            return vm;
        }
        public TViewModel ToViewModel<TViewModel>(TViewModel dbEntity) where TViewModel : class ,new()
        {
            Mn.Framework.Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TViewModel;
        }
    }
    public abstract class BaseEntity<TPrimaryKey> : IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {

        [Key]
        public virtual TPrimaryKey Id { get; set; }

        public TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new()
        {
            var vm = new TViewModel();
            vm = Mn.Framework.Helper.AutoMapper.Map<TViewModel>(this);
            return vm;
        }
        public TViewModel ToViewModel<TViewModel>(TViewModel dbEntity) where TViewModel : class ,new()
        {
            Mn.Framework.Helper.AutoMapper.Map(this, dbEntity);
            return dbEntity as TViewModel;
        }
    }
}
