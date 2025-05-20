using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoQueryResponseContract
    {
        public long Id { get; set; }
        public long TipoAtendimentoId { get; set; }
        public string TipoAtendimentoDescricao { get; set; }
        public long PessoaId { get; set; }
        public string PessoaNome { get; set; }
        public long UsuarioCriadorId { get; set; }
        public string UsuarioCriadorLogin { get; set; }
        public StatusAtendimentoEnum StatusAtendimento { get; set; }
        public DadosProximoContatoResponse ProximoContato { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class AtendimentoResponseContract
    {
        public long Id { get; set; }
        public long TipoAtendimentoId { get; set; }
        public string TipoAtendimentoDescricao { get; set; }
        public long PessoaId { get; set; }
        public string PessoaNome { get; set; }
        public long UsuarioCriadorId { get; set; }
        public string UsuarioCriadorLogin { get; set; }
        public StatusAtendimentoEnum StatusAtendimento { get; set; }
        public List<AtendimentoParecerResponseContract> Pareceres { get; set; }
        public DadosProximoContatoResponse ProximoContato { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class AtendimentoParecerResponseContract
    {
        public long Id { get; set; }
        public long TipoAtendimentoId { get; set; }
        public long PessoaId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public string UsuarioCriadorLogin { get; set; }
        public StatusAtendimentoEnum StatusAtendimento { get; set; }
        public List<Parecer> Pareceres { get; set; }
        public string Descricao { get; set; }
    }

    public class DadosProximoContatoResponse
    {
        public long UsuarioId { get; set; }
        public string UsuarioLogin { get; set; }
        public DateTime Data { get; set; }
    }
}