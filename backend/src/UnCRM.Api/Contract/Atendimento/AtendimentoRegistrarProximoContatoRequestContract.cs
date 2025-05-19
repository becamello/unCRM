using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoRegistrarProximoContatoRequestContract
    {
        public DateTime Data { get; set; }
        public long Usuario { get; set; }

         public async Task Validar()
        {
            var validator = new AtendimentoRegistrarProximoContatoRequestContractValidator();
            var results = await validator.ValidateAsync(this);

            if (!results.IsValid)
                throw new ValidationResultException("Ocorreu um ou mais erros de validação.", results.Errors.ToList());
        }

    }
}