namespace ECommerceDinoShop.DTO
{
    public class ResponseDTO<T>
    {
        public T? Result { get; set; }
        public bool IsCorrect { get; set; }
        public string? Message { get; set; }
    }
}
