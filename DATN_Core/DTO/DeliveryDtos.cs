namespace DATN_Core.DTO
{
    public class DeliveryDtos
    {

    }

    public class CreateDeliveryDtos
    {
        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; }
        
        public string Note { get; set; } = string.Empty;
    }
}
