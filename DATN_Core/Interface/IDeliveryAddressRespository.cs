using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_Core.Interface
{
    public interface IDeliveryAddressRespository: IGenericeReponsitory<DeliveryAddress>
    {
        Task<DeliveryAddress> CreateAsync(CreateDeliveryDtos model, int idOrder);
    }
}
