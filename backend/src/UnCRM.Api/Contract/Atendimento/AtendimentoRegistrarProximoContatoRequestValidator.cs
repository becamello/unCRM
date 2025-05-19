using FluentValidation;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoRegistrarProximoContatoRequestContractValidator : AbstractValidator<AtendimentoRegistrarProximoContatoRequestContract>
    {
        public AtendimentoRegistrarProximoContatoRequestContractValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("O usuário delegado é obrigatório.");

            RuleFor(x => x.Data)
                .NotEmpty().WithMessage("A data do próximo contato é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("A data do próximo contato deve ser igual ou futura à data atual.");
        }
    }
}
