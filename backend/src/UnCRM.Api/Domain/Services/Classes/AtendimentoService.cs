using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Contract.Atendimento;
using UnCRM.Api.Contract.Atendimento.Request;
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
            var atendimentos = await repository.Query()
                .Include(x => x.Pessoa)
                .Include(x => x.TipoAtendimento)
                .AsNoTracking()
                .ToListAsync();

            var usuarioIds = atendimentos
                .Select(x => x.UsuarioCriacaoId)
                .Concat(atendimentos
                    .Where(x => x.ProximoContato != null)
                    .Select(x => x.ProximoContato.Usuario))
                .Distinct()
                .ToList();

            var usuarios = await usuarioRepository.Query()
                .Where(u => usuarioIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Login);

            return atendimentos.Select(x => new AtendimentoQueryResponseContract
            {
                Id = x.Id,
                TipoAtendimentoId = x.TipoAtendimentoId,
                TipoAtendimentoDescricao = x.TipoAtendimento?.Descricao,

                PessoaId = x.PessoaId,
                PessoaNome = x.Pessoa?.Nome,

                UsuarioCriadorId = x.UsuarioCriacaoId,
                UsuarioCriadorLogin = usuarios.GetValueOrDefault(x.UsuarioCriacaoId),

                StatusAtendimento = x.Status,
                DataCadastro = x.DataCadastro,

                ProximoContato = x.ProximoContato is not null
                    ? new DadosProximoContatoResponse
                    {
                        UsuarioId = x.ProximoContato.Usuario,
                        UsuarioLogin = usuarios.GetValueOrDefault(x.ProximoContato.Usuario),
                        Data = x.ProximoContato.Data
                    }
                    : null
            }).ToList();
        }

        public async Task<AtendimentoResponseContract> ObterPorId(long id)
        {
            var atendimento = await repository.Query()
                .Include(x => x.Pessoa)
                .Include(x => x.TipoAtendimento)
                .Include(x => x.Pareceres)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Atendimento não encontrado");

            var usuarioIds = new List<long> { atendimento.UsuarioCriacaoId };
            if (atendimento.ProximoContato != null)
                usuarioIds.Add(atendimento.ProximoContato.Usuario);

            usuarioIds.AddRange(atendimento.Pareceres.Select(p => p.UsuarioCriacaoId));

            var usuarios = await usuarioRepository.Query()
                .Where(u => usuarioIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Login);

            return new AtendimentoResponseContract
            {
                Id = atendimento.Id,
                TipoAtendimentoId = atendimento.TipoAtendimentoId,
                TipoAtendimentoDescricao = atendimento.TipoAtendimento?.Descricao,

                PessoaId = atendimento.PessoaId,
                PessoaNome = atendimento.Pessoa?.Nome,

                UsuarioCriadorId = atendimento.UsuarioCriacaoId,
                UsuarioCriadorLogin = usuarios.GetValueOrDefault(atendimento.UsuarioCriacaoId),

                StatusAtendimento = atendimento.Status,
                DataCadastro = atendimento.DataCadastro,

                ProximoContato = atendimento.ProximoContato != null
                    ? new DadosProximoContatoResponse
                    {
                        UsuarioId = atendimento.ProximoContato.Usuario,
                        UsuarioLogin = usuarios.GetValueOrDefault(atendimento.ProximoContato.Usuario),
                        Data = atendimento.ProximoContato.Data
                    }
                    : null,

                Pareceres = atendimento.Pareceres.Select(p => new AtendimentoParecerResponseContract
                {
                    Id = p.Id,
                    Descricao = p.Descricao,
                    TipoAtendimentoId = atendimento.TipoAtendimentoId,
                    PessoaId = atendimento.PessoaId,
                    UsuarioCriadorId = p.UsuarioCriacaoId,
                    UsuarioCriadorLogin = usuarios.GetValueOrDefault(p.UsuarioCriacaoId),
                    StatusAtendimento = atendimento.Status,
                    Data = p.Data
                }).ToList()
            };
        }

        public async Task Atualizar(long id, long usuarioLogadoId, AtendimentoCriarRequestContract request)
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

            if (!string.IsNullOrWhiteSpace(request.Parecer))
            {
                atendimento.RegistrarParecer(usuarioLogadoId, request.Parecer);
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

        public async Task RegistrarProximoContato(long atendimentoId, AtendimentoRegistrarProximoContatoRequestContract request)
        {
            await request.Validar();

            var atendimento = await repository.Obter(atendimentoId)
                ?? throw new NotFoundException("Atendimento não encontrado");

            var proximoContato = new DadosProximoContato(request.Usuario, request.Data);

            atendimento.RegistrarProximoContato(proximoContato);

            await repository.Atualizar(atendimento);
        }

        public async Task RegistrarParecerEProximoContato(long id, long usuarioLogadoId, AtendimentoRegistrarParecerProximoContatoRequestContract request)
        {
            await request.Validar();

            var atendimento = await repository.Obter(id)
                ?? throw new NotFoundException("Atendimento não localizado");

            atendimento.RegistrarParecer(usuarioLogadoId, request.Parecer);

            var proximoContato = new DadosProximoContato(request.ProximoContato.Usuario, request.ProximoContato.Data);
            atendimento.RegistrarProximoContato(proximoContato);

            await repository.Atualizar(atendimento);
        }

        public async Task RegistrarParecerEEncerrar(long id, long usuarioLogadoId, AtendimentoRegistrarParecerRequestContract request)
        {
            var atendimento = await repository.Obter(id)
                ?? throw new NotFoundException("Atendimento não localizado");

            atendimento.RegistrarParecer(usuarioLogadoId, request.Parecer);
            atendimento.Encerrar();

            await repository.Atualizar(atendimento);
        }


    }
}
