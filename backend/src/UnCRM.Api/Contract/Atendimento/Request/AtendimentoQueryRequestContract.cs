using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoQueryRequestContract
    {
        public List<long> PessoaId { get; set; } = [];
        public List<long> UsuarioCriadorId { get; set; } = [];
        public DateTime? DataInicialCadastro { get; set; }
        public DateTime? DataFinalCadastro { get; set; }
        public List<long> UsuarioProximoContatoId { get; set; } = [];
        public DateTime? DataInicialProximoContato { get; set; }
        public DateTime? DataFinalProximoContato { get; set; }
    }
}