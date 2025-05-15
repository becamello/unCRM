namespace UnCRM.Api.Domain.Models
{
    public class Parecer : Entidade<long>
    {
        public long UsuarioCriacaoId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public long AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}