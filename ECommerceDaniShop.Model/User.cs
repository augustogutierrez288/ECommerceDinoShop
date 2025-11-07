using System;
using System.Collections.Generic;

namespace ECommerceDinoShop.Model;

public partial class User
{
    public int IdUser { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
