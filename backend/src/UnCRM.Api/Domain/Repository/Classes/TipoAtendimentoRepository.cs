using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public class TipoAtendimentoRepository(ApplicationContext context) : Repository<TipoAtendimento, long>(context), IRepository<TipoAtendimento, long>
    {
    }
}
