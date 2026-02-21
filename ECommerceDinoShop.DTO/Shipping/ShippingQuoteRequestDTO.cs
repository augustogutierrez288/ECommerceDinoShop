namespace ECommerceDinoShop.DTO.Shipping
{
    public class ShippingQuoteRequestDTO
    {
        public string ZipCode { get; set; }
        // Para simplificar, asumiremos un peso/volumen estándar o calculado por cantidad de productos
        public int TotalWeight { get; set; } = 1;
    }
}
