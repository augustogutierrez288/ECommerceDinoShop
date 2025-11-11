using System.ComponentModel.DataAnnotations;

namespace ECommerceDinoShop.DTO
{
    public class CategoryDTO
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Ingrese nombre categoria")]
        public string? Name { get; set; }
    }
}
