namespace UnCRM.Api.Domain.Models
{
    public class TipoAtendimento : Entidade<long>
    {
        public string Descricao { get; set; } = string.Empty;
    }
}