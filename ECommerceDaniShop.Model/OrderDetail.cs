using System;
using System.Collections.Generic;

namespace ECommerceDinoShop.Model;

public partial class OrderDetail
{
    public int IdOrderDetail { get; set; }

    public int? IdOrder { get; set; }

    public int? IdProduct { get; set; }

    public int? Quantity { get; set; }

    public decimal? Total { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
