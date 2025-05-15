namespace UnCRM.Api.Exceptions
{
    public class AtendimentoEncerradoException() : RegraNegocioException("O atendimento já se encontra encerrado", 400)
    {
    }
}
