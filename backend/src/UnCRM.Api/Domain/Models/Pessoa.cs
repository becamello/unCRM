using System.ComponentModel.DataAnnotations;
using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Domain.Models
{
    public class Pessoa
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "O campo de nome é obrigatório.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O campo de nome curto é obrigatório.")]
        public string NomeCurto { get; set; } = null!;

        [Required(ErrorMessage = "O campo de tipo da pessoa é obrigatório.")]
        public TipoPessoaEnum TipoPessoa { get; set; }

        [Required(ErrorMessage = "O campo do documento da pessoa é obrigatório.")]
        public string CpfCnpj { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        public DateTime? DataInativacao { get; set; }
    }
}