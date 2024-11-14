using DATN_Core.DTO;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Helper
{
    public class Pagination<T> where T : class
    {
        private int totalItems;
        private IReadOnlyList<ProductDTO> result;
        private IReadOnlyList<ProductDetailDTO> result1;

        public Pagination(int pageSize, int pageNumber, int pageCount, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = pageCount;
            Data = data;
        }

        public Pagination(int pagesize, int pageNumber, int totalItems, IReadOnlyList<ProductDTO> result)
        {
            PageSize = pagesize;
            PageNumber = pageNumber;
            this.totalItems = totalItems;
            this.result = result;
        }

        public Pagination(int pagesize, int pageNumber, int totalItems, IReadOnlyList<ProductDetailDTO> result1)
        {
            PageSize = pagesize;
            PageNumber = pageNumber;
            this.totalItems = totalItems;
            this.result1 = result1;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public IReadOnlyList<T> Data
        {
            get; set;
        }
    }
}
