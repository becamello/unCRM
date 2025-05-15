using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public class PessoaRepository(ApplicationContext context) : Repository<Pessoa, long>(context), IPessoaRepository
    {
        public override async Task Deletar(Pessoa entidade)
        {
            entidade.DataInativacao = DateTime.Now;
            await Atualizar(entidade);
        }
    }                   
}