using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Contract.Atendimento.Request
{
    public class AtendimentoRegistrarParecerProximoContatoRequestContract
    {
        public string Parecer { get; set; }
        public AtendimentoRegistrarProximoContatoRequestContract ProximoContato { get; set; }

        public async Task Validar()
        {
            var validator = new AtendimentoRegistrarParecerProximoContatoRequestContractValidator();
            var result = await validator.ValidateAsync(this);

            if (!result.IsValid)
                throw new ValidationResultException("Erros de validação encontrados.", result.Errors.ToList());
        }
    }
}