using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 8)]
        public string Password { get; set; }
        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
