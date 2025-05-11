using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public class UsuarioRepository(ApplicationContext context) : IUsuarioRepository
    {
        private readonly ApplicationContext _contexto = context;

        public async Task<Usuario> Adicionar(Usuario entidade)
        {
            try
            {
                await _contexto.Usuario.AddAsync(entidade);
                await _contexto.SaveChangesAsync();
                return entidade;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar no banco: " + ex.Message);
                throw;
            }
        }

        public async Task<Usuario> Atualizar(Usuario entidade)
        {
            Usuario entidadeBanco = _contexto.Usuario
                .Where(u => u.Id == entidade.Id)
                .FirstOrDefault();

            _contexto.Entry(entidadeBanco).CurrentValues.SetValues(entidade);
            _contexto.Update<Usuario>(entidadeBanco);

            await _contexto.SaveChangesAsync();

            return entidadeBanco;
        }

        public async Task Deletar(Usuario entidade)
        {
            entidade.DataInativacao = DateTime.Now;
            await Atualizar(entidade);
        }

        public async Task<Usuario> Obter(string login)
        {
            return await _contexto.Usuario.AsNoTracking()
                              .Where(u => u.Login == login)
                              .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> Obter()
        {
            return await _contexto.Usuario.AsNoTracking()
                                           .OrderBy(u => u.Id)
                                           .ToListAsync();
        }

        public async Task<Usuario> Obter(long id)
        {
            return await _contexto.Usuario.AsNoTracking()
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
        }
    }
}