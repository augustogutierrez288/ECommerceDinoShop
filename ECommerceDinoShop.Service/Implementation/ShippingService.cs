// ShippingService.cs
using ECommerceDinoShop.DTO.Shipping;
using ECommerceDinoShop.Service.Contract;

public class ShippingService : IShippingService
{
    // Aquí inyectarías IHttpClientFactory para llamar a la API real de PaqAr

    public async Task<List<ShippingOptionDTO>> QuoteShipping(string zipCode)
    {
        // LÓGICA MOCK (SIMULADA) PARA QUE FUNCIONE INMEDIATAMENTE
        // En producción: Aquí harías el POST a https://api.correoargentino.com.ar/v1/rates
 

        var options = new List<ShippingOptionDTO>();

        // Validar CP básico
        if (string.IsNullOrEmpty(zipCode) || zipCode.Length < 4) return options;

        // Simulamos precios base + variable por CP
        decimal basePrice = 4500;
        if (zipCode.StartsWith("1")) basePrice = 3500; // CABA/GBA más barato

        options.Add(new ShippingOptionDTO
        {
            Name = "Paq.Ar Clásico Sucursal",
            Description = "Retiro en sucursal de Correo Argentino más cercana",
            Price = basePrice,
            DeliveryEstimate = "3 a 6 días hábiles",
            ProductCode = "PAQ-SUC"
        });

        options.Add(new ShippingOptionDTO
        {
            Name = "Paq.Ar Clásico Domicilio",
            Description = "Entrega en tu domicilio",
            Price = basePrice * 1.3m, // 30% más caro
            DeliveryEstimate = "3 a 6 días hábiles",
            ProductCode = "PAQ-DOM"
        });

        options.Add(new ShippingOptionDTO
        {
            Name = "Paq.Ar Expreso Domicilio",
            Description = "Entrega prioritaria",
            Price = basePrice * 1.8m,
            DeliveryEstimate = "1 a 3 días hábiles",
            ProductCode = "PAQ-EXP"
        });

        return options;
    }
}