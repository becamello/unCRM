using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public class PessoaRepository(ApplicationContext context) : IPessoaRepository
    {
        private readonly ApplicationContext _contexto = context;

        public async Task<Pessoa> Adicionar(Pessoa entidade)
        {
            try
            {
                await _contexto.Pessoa.AddAsync(entidade);
                await _contexto.SaveChangesAsync();
                return entidade;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar no banco: " + ex.Message);

                throw;
            }
        }

        public async Task<Pessoa> Atualizar(Pessoa entidade)
        {
            var entidadeBanco = await _contexto.Pessoa
            .FirstOrDefaultAsync(u => u.Id == entidade.Id) ?? throw new Exception("Pessoa não encontrada para atualização.");
            _contexto.Entry(entidadeBanco).CurrentValues.SetValues(entidade);
            _contexto.Pessoa.Update(entidadeBanco);

            await _contexto.SaveChangesAsync();

            return entidadeBanco;
        }

        public async Task Deletar(Pessoa entidade)
        {
            entidade.DataInativacao = DateTime.Now;
            await Atualizar(entidade);
        }

        public async Task<IEnumerable<Pessoa>> Obter()
        {
            return await _contexto.Pessoa.AsNoTracking()
                                           .OrderBy(u => u.Id)
                                           .ToListAsync();
        }

        public async Task<Pessoa> Obter(long id)
        {
            return await _contexto.Pessoa.AsNoTracking()
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
        }
    }
}