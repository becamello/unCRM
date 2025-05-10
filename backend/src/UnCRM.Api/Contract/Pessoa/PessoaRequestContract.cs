using System.ComponentModel.DataAnnotations;
using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Contract.Pessoa
{
    public class PessoaRequestContract
    {
         public DateTime? DataInativacao { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo NomeCurto é obrigatório.")]
        public string NomeCurto { get; set; } = string.Empty;

        public string? Cpf { get; set; }

        public string? Cnpj { get; set; }

        [Required(ErrorMessage = "O campo TipoPessoa é obrigatório.")]
        public TipoPessoaEnum TipoPessoa { get; set; }
    }
}