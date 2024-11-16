using AutoMapper;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Helper
{
    public class CategoryResolvel : IValueResolver<Category, CataDTO, string>

    {
        private readonly IConfiguration _configuration;
        public CategoryResolvel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Category source, CataDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Image))
            {
                return _configuration["API_url"] + source.Image;
            }
            return null;
        }
    }
}
