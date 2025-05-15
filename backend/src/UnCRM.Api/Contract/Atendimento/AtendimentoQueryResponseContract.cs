using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoQueryResponseContract
    {
        public long Id { get; set; }
        public long TipoAtendimentoId { get; set; }
        public long PessoaId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public StatusAtendimentoEnum StatusAtendimento { get; set; }
        public DadosProximoContato ProximoContato { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class AtendimentoResponseContract
    {
        public long Id { get; set; }
        public long TipoAtendimentoId { get; set; }
        public long PessoaId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public StatusAtendimentoEnum StatusAtendimento { get; set; }
        public List<AtendimentoParecerResponseContract> Pareceres { get; set; }
        public DadosProximoContato ProximoContato { get; set; }
    }

    public class AtendimentoParecerResponseContract
    {
        public long Id { get; set; }
         public long TipoAtendimentoId { get; set; }
        public long PessoaId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public StatusAtendimentoEnum StatusAtendimento { get; set; }
        public List<Parecer> Pareceres { get; set; }
        public string Descricao { get; set; }
    }
}