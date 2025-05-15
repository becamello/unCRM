using UnCRM.Api.Domain.Models;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Contract.Atendimento
{
    public class AtendimentoCriarRequestContract
    {
        public long TipoAtendimentoId { get; set; }
        public string Parecer { get; set; }
        public long PessoaId { get; set; }
        public DadosProximoContato ProximoContato { get; set; }
        public async Task Validar()
        {
            var validator = new AtendimentoCriarRequestContractValidator();
            var results = await validator.ValidateAsync(this);

            if (!results.IsValid)
                throw new ValidationResultException("Ocorreu um ou mais erros de validação.", results.Errors.ToList());
        }

    }
}
