namespace UnCRM.Api.Domain.Repository.Interfaces
{
    public interface IRepository<T, Tid> where T : class
    {
        Task<IEnumerable<T>> Obter();
        Task<T> Obter(Tid id);
        Task<T> Adicionar(T entidade);
        Task<T> Atualizar(T entidade);
        Task Deletar(T entidade);
        IQueryable<T> Query();
    }
}