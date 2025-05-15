using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public class UsuarioRepository(ApplicationContext context) : Repository<Usuario, long>(context), IUsuarioRepository
    {
        public async Task<Usuario> Obter(string login)
        {
            return await context.Usuario
                .Where(x => x.Login == login)
                .FirstOrDefaultAsync();
        }

        public override async Task Deletar(Usuario entidade)
        {
            entidade.DataInativacao = DateTime.Now;
            await Atualizar(entidade);
        }
    }
}