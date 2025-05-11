using System.ComponentModel.DataAnnotations;
using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Contract.Pessoa
{
    public class PessoaRequestContract
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Nome Curto é obrigatório.")]
        public string NomeCurto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O documento da pessoa é obrigatório.")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "O campo TipoPessoa é obrigatório.")]
        public TipoPessoaEnum TipoPessoa { get; set; }
    }
}