using System.Text.Json.Serialization;

namespace ECommerceDinoShop.DTO
{
    public class MercadoPagoWebhookDTO
    {
        public string action { get; set; }

        
        public string api_version { get; set; }

       
        public MercadoPagoData data { get; set; }

        
        public DateTime? date_created { get; set; }

       
        public long id { get; set; }

       
        public bool live_mode { get; set; }

      
        public string type { get; set; }

       
        public long user_id { get; set; }
    }
}
