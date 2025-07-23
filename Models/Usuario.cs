using System.ComponentModel.DataAnnotations;

namespace MinhaApi.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Nome e obrigatorio")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo Nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Login e obrigatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Login deve ter entre 3 e 20 caracteres")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo Senha e obrigatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Senha deve ter entre 3 e 20 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Funcao e obrigatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Funcao deve ter entre 3 e 20 caracteres")]
        public string Funcao { get; set; }
    }
}