﻿using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class BlogReponsitory : GenericeReponsitory<Blog>, IBlogReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BlogReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddAsync(CreateBlogDTO blogDTO)
        {
            if (blogDTO.HeadLine != null)
            {
                var mediaEntity = _mapper.Map<Blog>(blogDTO);
                mediaEntity.AccountId = blogDTO.AccountId;

                await _context.Blogs.AddAsync(mediaEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ReturnBlogDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnBlogDTO();
            var query = await _context.Blogs.AsNoTracking().ToListAsync();
            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1)).Take(brandParams.Pagesize).ToList();
            result.BlogsDTO = _mapper.Map<List<BlogDTO>>(query);
            result.totalItems = query.Count;
            return result;


        }
        public async Task<bool> UpdateAsync(int id, UpdateDTO BlogDTO)
        {

            var currentBrand = await _context.Blogs.FindAsync(id);
            if (currentBrand != null)
            {
                _mapper.Map(BlogDTO, currentBrand);
                _context.Blogs.Update(currentBrand);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}