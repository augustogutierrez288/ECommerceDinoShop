namespace ECommerceDinoShop.DTO
{
    public class SendPaymentDTO
    {
        public string IdUser { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public required string PhoneAreaCode { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public string IdentificationType { get; set; } = string.Empty;
        public required string IdentificationNumber { get; set; } = string.Empty;
        public required string ZipCode { get; set; } = string.Empty;
        public required string StreetName { get; set; } = string.Empty;
        public required string StreetNumber { get; set; } = string.Empty;
        public List<CartItemShortDTO> Items { get; set; } = new();

        
        
        //public required string PersonType { get; set; }
        //public required string Description { get; set; }
        
        //public required string Neighborhood { get; set; }
        //public required string City { get; set; }
        //public required string FederalUnit { get; set; }
    }
}
