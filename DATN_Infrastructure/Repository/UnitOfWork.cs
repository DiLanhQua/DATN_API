using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryReponsitory CategoryReponsitory { get; }

        public IBrandReponsitory BrandReponsitory { get; }

        public IProductReponsitory ProductReponsitory { get; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryReponsitory = new CategoryReponsitory(_context);
            BrandReponsitory = new BrandReponsitory(_context);
            ProductReponsitory = new ProductReponsitory(_context);
        }
    }
}
