using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class ProductReponsitory : GenericeReponsitory<Product>, IProductReponsitory
    {
        public ProductReponsitory(ApplicationDbContext context) : base(context)
        {
        }
    }
}
