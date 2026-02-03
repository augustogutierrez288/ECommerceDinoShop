using ECommerceDinoShop.DTO;
using MercadoPago.Resource.Preference;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IPaymentService
    {
        Task<Preference> CreatePreferenceAsync(SendPaymentDTO model);
        bool ValidateWebhookSignature(string xSignature, string xRequestId, string queryDataId, string bodyId);
        Task<bool> ProcessPaymentAsync(long paymentId);
    }
}
