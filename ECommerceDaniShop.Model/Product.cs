using System;
using System.Collections.Generic;

namespace ECommerceDinoShop.Model;

public partial class Product
{
    public int IdProduct { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? IdCategory { get; set; }

    public decimal? Price { get; set; }

    public decimal? SalePrice { get; set; }

    public int? Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
