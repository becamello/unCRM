using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Domain.Models
{
    public class Atendimento : Entidade<long>
    {
        public StatusAtendimentoEnum Status { get; set; }
        public long TipoAtendimentoId { get; set; }
        public TipoAtendimento TipoAtendimento { get; set; }
        public long UsuarioCriacaoId { get; set; }
        public long PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }
        public DadosProximoContato ProximoContato { get; set; }
        public List<Parecer> Pareceres { get; set; } = [];
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataInativacao { get; set; }
        public void Encerrar()
        {
            Status = StatusAtendimentoEnum.Encerrado;
            DataInativacao = DateTime.Now;
        }
        public void Reabrir()
        {
            Status = StatusAtendimentoEnum.Aberto;
            DataInativacao = null;
        }
        public Parecer RegistrarParecer(long usuario, string descricao)
        {
            if (Status == StatusAtendimentoEnum.Encerrado)
                throw new AtendimentoEncerradoException();

            var parecer = new Parecer
            {
                UsuarioCriacaoId = usuario,
                Descricao = descricao,
                Data = DateTime.Now
            };

            Pareceres.Add(parecer);

            return parecer;
        }
        public Parecer EditarParecer(long parecerId, long usuarioId, string descricao)
        {
            if (Status == StatusAtendimentoEnum.Encerrado)
                throw new AtendimentoEncerradoException();

            var parecer = Pareceres.FirstOrDefault(x => x.Id == parecerId)
                ?? throw new NotFoundException("Parecer não localizado");

            parecer.Descricao = descricao;

            return parecer;
        }
        public void RegistrarProximoContato(DadosProximoContato proximoContato)
        {
            ProximoContato = proximoContato;
        }
        public static Atendimento Criar(long usuario, long tipo, string descricaoParecer, long pessoa)
        {
            var atendimento = new Atendimento()
            {
                Status = StatusAtendimentoEnum.Aberto,
                UsuarioCriacaoId = usuario,
                TipoAtendimentoId = tipo,
                PessoaId = pessoa,
            };

            atendimento.RegistrarParecer(usuario, descricaoParecer);

            return atendimento;
        }
    }
}