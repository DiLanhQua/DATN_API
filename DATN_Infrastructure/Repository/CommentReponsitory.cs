using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PdfSharpCore.Pdf.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class CommentRepository : GenericeReponsitory<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateCommentDTO commentDTO)
        {
            var comment = new Comment
            {
                Content = commentDTO.Content,
                ProductId = commentDTO.ProductId,
                AccountId = commentDTO.AccountId,
                TimeCreated = DateTime.Now,
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ReturnCommentDTO> GetAllAsync(Params commentParams)
        {
            var re = new ReturnCommentDTO();

            var query = await _context.Comments.Select(p => new CommentDTO
            {
                Content = p.Content,
                ProductId = p.ProductId,
                AccountId = p.AccountId,
                TimeCreated = DateTime.Now,
                ProductName = p.Product.ProductName,
                AccountName = p.Account.FullName,
            }).AsNoTracking().ToListAsync();

            if (!string.IsNullOrEmpty(commentParams.Search))
            {
                query = query.Where(c => c.Content.ToLower().Contains(commentParams.Search.ToLower())).ToList();
            }

            query = query
                .Skip(commentParams.Pagesize * (commentParams.PageNumber - 1))
                .Take(commentParams.Pagesize)
                .ToList();
            re.CommentsDTO = _mapper.Map<List<CommentDTO>>(query);
            re.totalItems = query.Count();
            return re;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCommentDTO commentDTO)
        {
            var currentComment = await _context.Comments.FindAsync(id);
            if (currentComment != null)
            {
                _mapper.Map(commentDTO, currentComment);
                _context.Comments.Update(currentComment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIsComment(CheckIsCommentDTO dto)
        {
            var historyByProduct = await _context.HistoryByProducts.Where(x => x.ProductId == dto.ProductId && x.AccountId == dto.AccountId && x.DetailProductId == dto.DetailProductId && x.Status == 1).FirstOrDefaultAsync();
            if (historyByProduct == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddComment(AddCommentDTO commentDTO)
        {
            if(commentDTO == null)
            {
                return false;
            }
            var checkIsCommentDTO = _mapper.Map<CheckIsCommentDTO>(commentDTO);
            var check = await CheckIsComment(checkIsCommentDTO);
            if(check == null || check == false)
            {
                return false;
            }
            var comment = _mapper.Map<Comment>(commentDTO);
            _context.Add(comment);
            if(await _context.SaveChangesAsync() > 0)
            {
                var historyByProduct = await _context.HistoryByProducts
                    .Where(x => x.ProductId == commentDTO.ProductId && x.DetailProductId == commentDTO.DetailProductId && x.AccountId == commentDTO.AccountId && x.Status == 1).FirstOrDefaultAsync();
                if (historyByProduct != null)
                {
                    historyByProduct.Status = 2;
                    _context.SaveChanges();
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<(List<GetCommentDTO> Comments, bool HasMore)> GetCommentByProductId(int id, int pageNumber)
        {
            int pageSize = 5;

            var commentsQuery = _context.Comments
                .Where(x => x.ProductId == id)
                .Include(x => x.DetailProduct)
                .Include(x => x.Account)
                .OrderByDescending(x => x.TimeCreated);

            // Tính toán số lượng bình luận cần lấy dựa trên pageNumber
            int skipCount = (pageNumber - 1) * pageSize;
            var comments = new List<Comment>();

            for (int i = 1; i <= pageNumber; i++)
            {
                var pageComments = await commentsQuery
                    .Skip((i - 1) * pageSize) // Bỏ qua các bình luận của các trang trước
                    .Take(pageSize) // Lấy bình luận của trang hiện tại
                    .ToListAsync();

                comments.AddRange(pageComments); // Thêm bình luận từ trang hiện tại vào danh sách
            }

            bool hasMore = comments.Count == pageNumber * pageSize; // Kiểm tra xem có còn bình luận để tải

            // Chuyển đổi thành DTO
            var commentDTOs = _mapper.Map<List<GetCommentDTO>>(comments);

            return (commentDTOs, hasMore); // Trả về cả danh sách bình luận và cờ xác định có còn bình luận để tải không
        }

        public async Task<double> GetRatingByProductId(int productId)
        {
            var comment = await _context.Comments
                .Where(x => x.ProductId == productId).ToListAsync();
            if (!comment.Any())
            {
                return 0;
            }
            double productRating = Math.Round(comment.Average(x => x.Rating), 1);

            return productRating;
        }





    }
}
