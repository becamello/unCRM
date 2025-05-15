using FluentValidation;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoCriarRequestContractValidator : AbstractValidator<AtendimentoCriarRequestContract>
    {
        public AtendimentoCriarRequestContractValidator()
        {
            RuleFor(x => x.TipoAtendimentoId)
                .NotEmpty().WithMessage("O Tipo de Atendimento é obrigatório.");
                
            RuleFor(x => x.PessoaId)
                .NotEmpty().WithMessage("A pessoa do atendimento é obrigatória.");

            RuleFor(x => x.Parecer)
                .NotEmpty().WithMessage("O parecer é obrigatório.")
                .MaximumLength(500).WithMessage("O tamanho máximo do parecer é 500");
        }
    }
}
