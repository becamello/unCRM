using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Contract.Atendimento;
using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Domain.Services.Classes
{
    public class AtendimentoService(IAtendimentoRepository repository, IUsuarioRepository usuarioRepository)
    {
        public async Task<AtendimentoCriarResponseContract> Criar(AtendimentoCriarRequestContract request, long usuarioLogadoId)
        {
            await request.Validar();

            var atendimento = Atendimento.Criar(
                usuarioLogadoId,
                request.TipoAtendimentoId,
                request.Parecer,
                request.PessoaId);

            if (request.ProximoContato != null)
            {
                var proximoContato = new DadosProximoContato(request.ProximoContato.Usuario, request.ProximoContato.Data);
                atendimento.RegistrarProximoContato(proximoContato);
            }

            await repository.Adicionar(atendimento);

            return new AtendimentoCriarResponseContract { Id = atendimento.Id };
        }

        public async Task<AtendimentoRegistrarParecerResponseContract> RegistrarParecer(long id, long usuarioLogadoId, AtendimentoRegistrarParecerRequestContract request)
        {
            var atendimento = await repository.Obter(id)
                ?? throw new NotFoundException("Atendimento não localizado");

            var parecer = atendimento.RegistrarParecer(
                usuarioLogadoId,
                request.Parecer);

            await repository.Atualizar(atendimento);

            return new AtendimentoRegistrarParecerResponseContract
            {
                ParecerId = parecer.Id,
                Parecer = request.Parecer,
                UsuarioCriadorId = usuarioLogadoId
            };
        }

        public async Task<IEnumerable<AtendimentoQueryResponseContract>> ObterTodos()
        {
            return await repository.Query()
                .AsNoTracking()
                .Select(x => new AtendimentoQueryResponseContract
                {
                    Id = x.Id,
                    TipoAtendimentoId = x.TipoAtendimentoId,
                    PessoaId = x.PessoaId,
                    UsuarioCriadorId = x.UsuarioCriacaoId,
                    StatusAtendimento = x.Status,
                    ProximoContato = x.ProximoContato,
                    DataCadastro = x.DataCadastro
                })
                .ToListAsync();
        }

        public async Task<AtendimentoResponseContract> ObterPorId(long id)
        {
            var atendimento = await repository.Query()
                .Include(x => x.Pareceres)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Atendimento não encontrado");

            return new AtendimentoResponseContract
            {
                Id = atendimento.Id,
                TipoAtendimentoId = atendimento.TipoAtendimentoId,
                PessoaId = atendimento.PessoaId,
                UsuarioCriadorId = atendimento.UsuarioCriacaoId,
                StatusAtendimento = atendimento.Status,
                ProximoContato = atendimento.ProximoContato,
                Pareceres = atendimento.Pareceres.Select(x => new AtendimentoParecerResponseContract
                {
                    Id = x.Id,
                    Descricao = x.Descricao
                }).ToList()
            };
        }

        public async Task Atualizar(long id, AtendimentoCriarRequestContract request)
        {
            var atendimento = await repository.Obter(id)
                ?? throw new NotFoundException("Atendimento não encontrado");

            atendimento.TipoAtendimentoId = request.TipoAtendimentoId;
            atendimento.PessoaId = request.PessoaId;

            if (request.ProximoContato != null)
            {
                var proximoContato = new DadosProximoContato(request.ProximoContato.Usuario, request.ProximoContato.Data);
                atendimento.RegistrarProximoContato(proximoContato);
            }

            await repository.Atualizar(atendimento);
        }

        public async Task<AtendimentoEditarParecerResponseContract> EditarParecer(long atendimentoId, long parecerId, AtendimentoEditarParecerRequestContract request, long usuarioLogadoId)
        {
            var atendimento = await repository.Obter(atendimentoId)
                ?? throw new NotFoundException("Atendimento não encontrado");

            var parecer = atendimento.EditarParecer(
                parecerId,
                usuarioLogadoId,
                request.Descricao
            );

            await repository.Atualizar(atendimento);

            return new AtendimentoEditarParecerResponseContract
            {
                ParecerId = parecer.Id,
                Parecer = parecer.Descricao,
                UsuarioCriadorId = parecer.UsuarioCriacaoId
            };
        }

        public async Task Reabrir(long id, long usuarioLogadoId)
        {
            var atendimento = await repository.Obter(id)
                ?? throw new NotFoundException("Atendimento não encontrado");

            var usuario = await usuarioRepository.Obter(usuarioLogadoId)
                ?? throw new NotFoundException("Usuário não encontrado");

            if (usuario.Cargo != CargoEnum.Gerente)
                throw new UnauthorizedException("Apenas usuários com perfil Gerente podem reabrir atendimentos.");

            atendimento.Reabrir();
            await repository.Atualizar(atendimento);
        }

        public async Task Encerrar(long id)
        {
            var atendimento = await repository.Obter(id)
                ?? throw new NotFoundException("Atendimento não encontrado");

            atendimento.Encerrar();

            await repository.Atualizar(atendimento);
        }
    }

    public class AtendimentoRegistrarParecerRequestContract
    {
        public string Parecer { get; set; }
    }

    public class AtendimentoRegistrarParecerResponseContract
    {
        public long ParecerId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public string Parecer { get; set; }
    }

    public class AtendimentoEditarParecerRequestContract
    {
        public long ParecerId { get; set; }
        public string Descricao { get; set; }
    }

    public class AtendimentoEditarParecerResponseContract
    {
        public long ParecerId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public string Parecer { get; set; }
    }
}
