using System.Text.Json.Serialization;

namespace ECommerceDinoShop.DTO
{
    public class MercadoPagoWebhookDTO
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; }

        [JsonPropertyName("data")]
        public MercadoPagoData Data { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("live_mode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }
    }
}
