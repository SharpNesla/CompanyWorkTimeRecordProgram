using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.ViewModelBases
{
    public class InfoViewModelBase<TEntity> : ViewModelBase
    {
        public virtual string InfoTitle { get; }
        public TEntity Entity { get; set; }
    }
}
