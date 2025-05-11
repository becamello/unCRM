namespace UnCRM.Api.Domain.Services.Interfaces
{
    /// <summary>
    ///Interface genérica para criação de serviços do tipo CRUD.
    /// </summary>
    /// <typeparam name="RQ">Contrato de request</typeparam>
    /// <typeparam name="RS">Contrato de response</typeparam>
    /// <typeparam name="I">Tipo do Id</typeparam>
    public interface IService<RQ, RS, I> where RQ : class
    {
        Task<IEnumerable<RS>> ObterTodos();
        Task<RS> ObterPorId(I id);
        Task<RS> Adicionar(RQ entidade);
        Task<RS> Atualizar(I id, RQ entidade);
        Task Inativar(I id);
    }
}