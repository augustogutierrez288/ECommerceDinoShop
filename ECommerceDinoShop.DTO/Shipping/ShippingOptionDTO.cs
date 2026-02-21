namespace ECommerceDinoShop.DTO.Shipping
{
    public class ShippingOptionDTO
    {
        public string Name { get; set; } // "Clásico a Domicilio", "Expreso Sucursal"
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DeliveryEstimate { get; set; }
        public string ProductCode { get; set; } // Código interno de PaqAr
    }
}
