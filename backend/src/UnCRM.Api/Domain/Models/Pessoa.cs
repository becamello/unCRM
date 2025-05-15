using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Domain.Models
{
    public class Pessoa : Entidade<long>
    {
        public string Nome { get; set; } = null!;
        public string NomeCurto { get; set; } = null!;
        public TipoPessoaEnum TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataInativacao { get; set; }
    }
}