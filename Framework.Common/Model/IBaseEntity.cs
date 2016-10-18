using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Common.Model
{
    public interface IBaseEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        TPrimaryKey Id { get; set; }
        TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new();
    }
    public interface IBaseEntity : IBaseEntity<Int64>
    {
        Int64 Id { get; set; }
        TViewModel ToViewModel<TViewModel>() where TViewModel : class ,new();
    }
}
