using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Common.Model
{
    public class MetaData
    {
        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; } // even thought not required, EF designates the col as not null, must provide value
        public bool IsDeleted { get; set; }
    }
}
