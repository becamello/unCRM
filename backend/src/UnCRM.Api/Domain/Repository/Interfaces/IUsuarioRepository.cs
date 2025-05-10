using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Domain.Repository.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario, long>
    {
        Task<Usuario?> Obter(string login);
    }
}