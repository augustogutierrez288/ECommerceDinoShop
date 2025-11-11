namespace ECommerceDinoShop.DTO
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }

        public int? IdUser { get; set; }

        public decimal? Total { get; set; }

        public virtual ICollection<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }
}
