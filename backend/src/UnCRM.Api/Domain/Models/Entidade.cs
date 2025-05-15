namespace UnCRM.Api.Domain.Models
{
    public abstract class Entidade<TId>
    {
        public TId Id { get; set; }
    }
}
