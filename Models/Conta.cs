using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApi.Models
{
    public class Conta
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Descriçao e obrigatorio")]
        [StringLength(200, ErrorMessage = "O campo Descriçao pode ter ate 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo Valor e obrigatorio")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo DataVentimento e obrigatorio")]
        [DataType(DataType.Date)]
        public DateTime DataVentimento { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataPagamento { get; set; }

        [Required(ErrorMessage = "O campo Situacao e obrigatorio")]
        public Situacao Situacao { get; set; }

        [Required(ErrorMessage = "O campo Pessoa e obrigatorio")]
        public Guid PessoaId { get; set; }

        public Conta()
        {
            Id = Guid.NewGuid();
        }

        //Relacionamento Entity Framework
        public Pessoa Pessoa { get; set; }
    }
}