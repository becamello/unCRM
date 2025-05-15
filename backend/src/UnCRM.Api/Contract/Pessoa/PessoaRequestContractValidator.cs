using FluentValidation;
using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Utils;

namespace UnCRM.Api.Contract.Pessoa
{
    public class PessoaRequestContractValidator : AbstractValidator<PessoaRequestContract>
    {
        public PessoaRequestContractValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O campo Nome é obrigatório.");

            RuleFor(x => x.NomeCurto)
                .NotEmpty().WithMessage("O campo Nome Curto é obrigatório.");

            RuleFor(x => x.TipoPessoa)
                .IsInEnum().WithMessage("O campo TipoPessoa é obrigatório.");

            RuleFor(x => x.CpfCnpj)
                .Must(DocumentoUtils.ValidarCpf)
                .WithMessage("O documento informado não é um CPF válido.")
                .When(x => x.TipoPessoa == TipoPessoaEnum.PessoaFisica);

            RuleFor(x => x.CpfCnpj)
                .Must(DocumentoUtils.ValidarCnpj)
                .WithMessage("O documento informado não é um CNPJ válido.")
                .When(x => x.TipoPessoa == TipoPessoaEnum.PessoaJuridica);
        }
    }
}
