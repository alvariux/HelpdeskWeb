using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La nueva contraseña debe tener al menos {2} caracteres.", MinimumLength = 12)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "La confirmación de la nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las nuevas contraseñas no coinciden.")]
        [Display(Name = "Confirmar nueva contraseña")]
        public string ConfirmNewPassword { get; set; }
    }
}
