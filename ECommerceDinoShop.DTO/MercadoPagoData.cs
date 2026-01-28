using System.Text.Json.Serialization;

namespace ECommerceDinoShop.DTO
{
    public class MercadoPagoData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
