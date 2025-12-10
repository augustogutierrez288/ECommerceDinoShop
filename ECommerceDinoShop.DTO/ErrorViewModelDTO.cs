namespace ECommerceDinoShop.DTO
{
    public class ErrorViewModelDTO
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
