using System.ComponentModel.DataAnnotations;

namespace MinhaApi.Models
{
    public class Estado
    {
        [Key]
        [StringLength(2,MinimumLength =2, ErrorMessage = "O campo Sigla deve ter 2 caracteres")]
        public string Sigla { get; set; }

        [Required(ErrorMessage = "O campo Nome e obgritagorio")]
        [StringLength(60,MinimumLength = 3, ErrorMessage = "O campo NOme deve ter entre 3 e 60 caracteres")]
        public string Nome { get; set; }
    }
}