using AutoMapper;
using AutoMapper.Execution;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DATN_API.Helper
{
    public class BrandResolver : IValueResolver<Brand, BrandDTO, string>

    {
        private readonly IConfiguration _configuration;
        public BrandResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Brand source, BrandDTO destination, string destMember, ResolutionContext context)
        {
           if(!string.IsNullOrEmpty(source.Image))
           {
                return _configuration["API_url"]+source.Image;
            }
            return null;
        }
    }
}
