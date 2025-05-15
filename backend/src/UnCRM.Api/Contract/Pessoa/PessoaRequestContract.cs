using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Contract.Pessoa
{
    public class PessoaRequestContract
    {
        public string Nome { get; set; } = string.Empty;
        public string NomeCurto { get; set; } = string.Empty;
        public string CpfCnpj { get; set; }
        public TipoPessoaEnum TipoPessoa { get; set; }
        public async Task Validar()
        {
            var validator = new PessoaRequestContractValidator();
            var results = await validator.ValidateAsync(this);

            if (!results.IsValid)
                throw new ValidationResultException("Ocorreu um ou mais erros de validação.", results.Errors.ToList());
        }
    }
}