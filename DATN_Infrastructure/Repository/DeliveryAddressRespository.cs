
using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;

namespace DATN_Infrastructure.Repository
{
    public class DeliveryAddressRespository : GenericeReponsitory<DeliveryAddress>, IDeliveryAddressRespository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeliveryAddressRespository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<DeliveryAddress> CreateAsync(CreateDeliveryDtos model, int idOrder)
        {
            DeliveryAddress data = _mapper.Map<DeliveryAddress>(model);

            data.OrderId = idOrder;
            data.ZipCode = GenerateRandomString();

            _context.Add(data);
            await _context.SaveChangesAsync();

            return data;
        }

        private string GenerateRandomString()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }
    }
}
