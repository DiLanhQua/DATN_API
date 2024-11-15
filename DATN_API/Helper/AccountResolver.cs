using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Helper
{
    public class AccountResolver : IValueResolver<Account, AccountDTO, string>

    {
        private readonly IConfiguration _configuration;
        public AccountResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Account source, AccountDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Image))
            {
                return _configuration["API_url"] + source.Image;
            }
            return null;
        }
    }
}
