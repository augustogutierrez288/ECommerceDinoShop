namespace ECommerceDinoShop.DTO
{
    public class CartDTO
    {
        public ProductDTO? Product { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
    }
}
