using System.ComponentModel.DataAnnotations;

namespace MinhaApi.Models
{
    public class UsuarioLogin
    {
        [Required(ErrorMessage = "O campo Login e obrigatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Login deve ter entre 3 e 20 caracteres")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo Senha e obrigatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Senha deve ter entre 3 e 20 caracteres")]
        public string Password { get; set; }
    }
}