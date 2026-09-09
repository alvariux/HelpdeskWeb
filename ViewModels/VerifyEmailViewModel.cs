using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email no es válido.")]
        public string Email { get; set; }
    }
}
