namespace ECommerceDinoShop.DTO
{
    public class OrderDetailDTO
    {
        public int IdOrderDetail { get; set; }

        public int? IdOrder { get; set; }

        public int? IdProduct { get; set; }

        public int? Quantity { get; set; }

        public decimal? Total { get; set; }
    }
}
