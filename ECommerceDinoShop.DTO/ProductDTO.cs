using System.ComponentModel.DataAnnotations;

namespace ECommerceDinoShop.DTO
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Ingrese nombre del producto")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Ingrese descripcion")]
        public string? Description { get; set; }

        public int? IdCategory { get; set; }

        [Required(ErrorMessage = "Ingrese precio")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Ingrese precio de oferta")]
        public decimal? SalePrice { get; set; }

        [Required(ErrorMessage = "Ingrese la cantidad")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Ingrese imagen")]
        public string? ImageUrl { get; set; }
        public DateTime? FechaCreacioCreatedAtn { get; set; }
        public virtual CategoryDTO? IdCategoryNavigation { get; set; }
    }
}
