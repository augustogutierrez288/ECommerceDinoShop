using System.ComponentModel.DataAnnotations;

namespace ECommerceDinoShop.DTO
{
    public class CardDTO
    {
        [Required(ErrorMessage = "Ingrese titular")]
        public string? Holder { get; set; }
        [Required(ErrorMessage = "Ingrese numero tarjeta")]
        public string? Number { get; set; }
        [Required(ErrorMessage = "Ingrese vigencia")]
        public string? Validity { get; set; }
        [Required(ErrorMessage = "Ingrese CVV")]
        public string? CVV { get; set; }
    }
}
