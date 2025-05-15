using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Domain.Models
{
    public class Usuario : Entidade<long>
    {
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public CargoEnum Cargo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataInativacao { get; set; }
    }
}