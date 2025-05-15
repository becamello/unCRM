using FluentValidation.Results;

namespace UnCRM.Api.Exceptions
{
    public class ValidationResultException(string message, List<ValidationFailure> errors) : Exception(message)
    {
        public List<ValidationFailure> Errors { get; } = errors;
    }
}
