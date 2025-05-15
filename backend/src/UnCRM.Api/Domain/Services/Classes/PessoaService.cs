using AutoMapper;
using UnCRM.Api.Contract.Pessoa;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;
using UnCRM.Api.Domain.Services.Interfaces;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Domain.Services.Classes
{
    public class PessoaService(IPessoaRepository repository, IMapper mapper) : IPessoaService
    {
        public async Task<PessoaResponseContract> Adicionar(PessoaRequestContract request)
        {
            await request.Validar();

            var pessoa = mapper.Map<Pessoa>(request);
            pessoa.DataCadastro = DateTime.Now;

            pessoa = await repository.Adicionar(pessoa);

            return mapper.Map<PessoaResponseContract>(pessoa);
        }


        public async Task<PessoaResponseContract> Atualizar(long id, PessoaRequestContract request)
        {
            await request.Validar();

            var entidade = await repository.Obter(id) 
                ?? throw new NotFoundException("Pessoa não encontrada.");

            mapper.Map(request, entidade);

            await repository.Atualizar(entidade);

            return mapper.Map<PessoaResponseContract>(entidade);
        }


        public async Task Inativar(long id)
        {
            var pessoa = await repository.Obter(id) 
                ?? throw new NotFoundException("Pessoa não encontrada para inativação.");

            await repository.Deletar(pessoa);
        }

        public async Task<IEnumerable<PessoaResponseContract>> ObterTodos()
        {
            var pessoas = await repository.Obter();

            return pessoas.Select(mapper.Map<PessoaResponseContract>);
        }

        public async Task<PessoaResponseContract> ObterPorId(long id)
        {
            var pessoa = await repository.Obter(id) 
                ?? throw new NotFoundException("Pessoa não encontrada.");

            return mapper.Map<PessoaResponseContract>(pessoa);
        }
    }
}