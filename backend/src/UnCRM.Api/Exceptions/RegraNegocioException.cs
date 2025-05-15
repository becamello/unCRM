namespace UnCRM.Api.Exceptions
{
    public class RegraNegocioException(string message, int status) : Exception(message)
    {
        public int Status { get; } = status;
    }
}
