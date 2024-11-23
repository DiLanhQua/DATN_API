using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Helper
{
    public class ImageResolver : IValueResolver<Image, ImageDeDTO, string>
    {

        private readonly IConfiguration _configuration;
        public ImageResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

<<<<<<< Updated upstream
        public string Resolve(Image source, ImageDeDTO destination, string destMember, ResolutionContext context)
=======
        public string Resolve(Image source, ImageDeDTO destination,string destMember, ResolutionContext context)
>>>>>>> Stashed changes
        {
            if (!string.IsNullOrEmpty(source.Link))
            {
                return _configuration["API_url"] + source.Link;
            }
            return null;
        }
    }
}
