using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public abstract class Repository<T, TId>(ApplicationContext context) : IRepository<T, TId> where T : Entidade<TId>
    {
        protected readonly ApplicationContext context = context;

        public async Task<T> Adicionar(T entidade)
        {
            await context.Set<T>().AddAsync(entidade);
            await context.SaveChangesAsync();

            return entidade;
        }

        public async Task<T> Atualizar(T entidade)
        {
            context.Set<T>().Update(entidade);
            await context.SaveChangesAsync();

            return entidade;
        }

        public virtual async Task Deletar(T entidade)
        {
            context.Set<T>().Remove(entidade);
            await context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> Obter()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> Obter(TId id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public IQueryable<T> Query() => context.Set<T>();
    }

}
