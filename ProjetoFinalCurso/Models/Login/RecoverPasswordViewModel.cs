using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models.Login
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
