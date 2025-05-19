namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoEditarParecerRequestContract
    {
        public long ParecerId { get; set; }
        public string Descricao { get; set; }
    }
}