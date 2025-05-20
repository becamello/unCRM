using FluentValidation;
using UnCRM.Api.Contract.Atendimento.Request;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoRegistrarParecerProximoContatoRequestContractValidator : AbstractValidator<AtendimentoRegistrarParecerProximoContatoRequestContract>
    {
        public AtendimentoRegistrarParecerProximoContatoRequestContractValidator()
        {
            RuleFor(x => x.Parecer)
                .NotEmpty().WithMessage("O parecer é obrigatório.");

            RuleFor(x => x.ProximoContato)
                .NotNull().WithMessage("Os dados do próximo contato são obrigatórios.")
                .SetValidator(new AtendimentoRegistrarProximoContatoRequestContractValidator());
        }
    }
}
