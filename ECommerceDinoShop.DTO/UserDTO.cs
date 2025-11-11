using System.ComponentModel.DataAnnotations;

namespace ECommerceDinoShop.DTO
{
    public class UserDTO
    {
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Ingrese nombre completo")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Ingrese correo")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Ingrese contraseña")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Ingrese confirmar contraseña")]
        public string? PasswordConfirms { get; set; }

        public string? Role { get; set; }
    }
}
