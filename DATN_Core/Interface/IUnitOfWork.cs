using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IUnitOfWork
    {
        public ICategoryReponsitory CategoryReponsitory { get; }    
        public IBrandReponsitory BrandReponsitory { get; }

        public IProductReponsitory ProductReponsitory { get; }
    }
}
