﻿using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class DetailProductReponsitory : GenericeReponsitory<DetailProduct>, IDetailProductReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DetailProductReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(ProductDetailDTO ProDTO)
        {
            var Productdetail = new DetailProduct
            {
                Size = ProDTO.Size,
                Price = ProDTO.Price,
                Quantity = ProDTO.Quantity,
                ColorId = ProDTO.ColorId,
                Gender = ProDTO.Gender,
                Status = ProDTO.Status,

                ProductId = ProDTO.ProductId
            };

            using var consaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.DetailProducts.AddAsync(Productdetail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                await consaction.RollbackAsync();
                throw new Exception("Đã xảy ra lỗi khi thêm sản phẩm.", ex);
            }






        }

        public async Task<bool> Deleteproduct(int id)
        {
            var query = await _context.DetailProducts.FindAsync(id);

            if (query != null)
            {
                _context.DetailProducts.Remove(query);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;

        }

        public async Task<ReturnProductDetailDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnProductDetailDTO();
            var query = await _context.DetailProducts.AsNoTracking().ToListAsync();


            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1)).Take(brandParams.Pagesize).ToList();
            result.Productdetail = _mapper.Map<List<ProductDetailDTO>>(query);
            result.TotalItems = query.Count;
            return result;

        }

        public async Task<List<ProductDetailDE>> GetProductDetail(int productid)
        {
            var query = await _context.DetailProducts.Where(p => p.ProductId == productid).Include(x=> x.Color).ToListAsync();
            return _mapper.Map<List<ProductDetailDE>>(query);
        }
        public async Task<ProductDetailDE> GetDetail(int id,int productid)
        {
            var query = await _context.DetailProducts.FirstOrDefaultAsync(p => p.Id == id && p.ProductId == productid);
            return _mapper.Map<ProductDetailDE>(query);
        }

        public async Task<bool> UpdateAsync(int id, ProductDetailUP ProDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var exiting = await _context.DetailProducts.FirstOrDefaultAsync(x => x.Id == id);

                if (exiting == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm");
                }

                exiting.Size = ProDTO.Size;
                exiting.Price = ProDTO.Price;
                exiting.Quantity = ProDTO.Quantity;
                exiting.ColorId = ProDTO.ColorId;
                exiting.Gender = ProDTO.Gender;

                _context.DetailProducts.Update(exiting);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("An error occurred while updating the product.", ex);
            }


        }

        public async Task<bool> CreateDetail(int idProduct, CreateDetail modal)
        {
            try
            {
                DetailProduct detailProduct = new DetailProduct
                {
                    Size = modal.Size,
                    Quantity = modal.Quantity,
                    ColorId = modal.ColorId,
                    Gender = modal.Gender,
                    Price = modal.Price,
                    Status = "1",
                    ProductId = idProduct,
                };

                _context.DetailProducts.Add(detailProduct);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
               return false;
            }
        }
    }
}
