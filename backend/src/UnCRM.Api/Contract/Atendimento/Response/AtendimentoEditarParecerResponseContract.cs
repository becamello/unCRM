namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoEditarParecerResponseContract
    {
        public long ParecerId { get; set; }
        public long UsuarioCriadorId { get; set; }
        public string Parecer { get; set; }
    }
}