using System;
using System.Collections.Generic;

namespace ECommerceDinoShop.Model;

public partial class Order
{
    public int IdOrder { get; set; }

    public int? IdUser { get; set; }

    public decimal? Total { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
