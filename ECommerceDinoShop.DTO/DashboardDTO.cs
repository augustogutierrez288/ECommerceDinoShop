using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceDinoShop.DTO
{
    public class DashboardDTO
    {
        public string? TotalRevenue { get; set; } //Ingresos totales
        public int TotalOrders { get; set; } //Total de ventas
        public int TotalCustomers { get; set; } //Clientes totales
        public int TotalProducts { get; set; } //Productos totales
    }
}
