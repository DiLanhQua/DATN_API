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

        public ICartReponsitory CartReponsitory { get; }
        public IProductReponsitory ProductReponsitory { get; }

        public InImageReponsitory InImageReponsitory { get; }
        public IOrderReponsitory OrderReponsitory { get; }
        public IDetailOrderReponsitory DetailOrderReponsitory { get; }
        public IDetailProductReponsitory DetailProductReponsitory { get; }
        public IBlogReponsitory BlogReponsitory { get; }
        public ICommentRepository CommentRepository { get; }
        public IVoucherRepository VoucherRepository { get; }
        public IAccountReponsitory AccountReponsitory { get; }
        public IEmail EmailReponsitory { get; }

    }
}
