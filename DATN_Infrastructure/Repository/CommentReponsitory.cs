using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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


    }
}
